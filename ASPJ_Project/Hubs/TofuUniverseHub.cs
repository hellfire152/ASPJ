using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ASPJ_Project.Models;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;

namespace ASPJ_Project.TofuUniverse
{   
    //[TofuAuthorize]
    public class TofuUniverseHub : Hub
    {
        public int SaveProgress(ProgressData progress)
        {
            //if connection is already invalidated
            if (ValidityMap.CurrentInstance.Contains(Context.ConnectionId) && 
                !ValidityMap.CurrentInstance[Context.ConnectionId])
            {
                Debug.WriteLine("INVALID CONNECTION");
                return -1;
            }


            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);

            //current time in UTC
            long utcTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

            //check database if it's too soon
            Database d = Database.CurrentInstance; long[] times = new long[3];
            DataTable dt = d.PRQ("SELECT * FROM savetime WHERE userID = @1", c);
            if (dt == null) return -2; //if database not up
            if(dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                times[0] = dr.Field<long>("time1");
                times[1] = dr.Field<long>("time2");
                times[2] = dr.Field<long>("time3");
            }
            else
            {
                d.PNQ("INSERT INTO savetime (userID, time1, time2, time3) VALUES (@1, @2, @3, @4)",
                    c, 0, 0, utcTime);
                times = new long[] { 0, 0, utcTime};
            }
            if (utcTime - times[0] < 60000) //4th save in a minute
            {
                return 0;
            } else
            {
                times[0] = utcTime;
                Array.Sort(times);
                d.PNQ("UPDATE savetime SET time1 = @1, time2 = @2, time3 = @3 WHERE userID = @4",
                    c, times[0], times[1], times[2]);
            }

            //get previous save data
            SaveFile prevSave;
            try
            {
                string prevSaveText = System.IO.File.ReadAllText(
                        dataRoot + "\\Saves\\" + c + ".tusav");
                prevSave = SaveFile.Parse(prevSaveText);
            } catch (FileNotFoundException e) //no existing save 
            {
                long defaultTime = utcTime - 100000; //100 seconds leeway
                prevSave = new SaveFile(defaultTime, 0,
                    new Dictionary<int, int>(), new int[] { });
            }

            bool noCheats = ProgressVerifier.VerifyProgress(prevSave, progress, utcTime);
            Debug.WriteLine("IS CHEATING: " + !noCheats);
            //verify progress
            if (!noCheats)
            {
                //if caught cheating
                //insert cheat record into database
                d.PNQ("INSERT INTO cheatlog (username, time) VALUES (@1, @2)", c, utcTime);
                ValidityMap.CurrentInstance[Context.ConnectionId] = false;
                return -1;
            }

            //save + time on first line
            string s = "" + utcTime
                + '\n' + progress.ToString();
            Debug.WriteLine("SAVING FOR " + c +":\n" + s);

            //write to file
            System.IO.File.WriteAllText(
                dataRoot + "\\Saves\\" + c + ".tusav", s);

            return 1;
        }

        //gets a save file and sends it to the client
        public string RequestSave()
        {

            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
               Context.RequestCookies["username"].Value);
            if (c == null || c == "guest")
            {
                return "invalid:No username attached";
            } else
            {
                Debug.Write("GETTING SAVE FILE OF: " + c);
                string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Debug.WriteLine(dataRoot);
                string saveFileLocation = dataRoot + "\\Saves\\" + c + ".tusav";
                if (File.Exists(saveFileLocation))
                {
                    //get savefile
                    string s = System.IO.File.ReadAllText(saveFileLocation);

                    //remove time from save
                    string[] saveParts = s.Split('\n');

                    //get current time
                    long utcTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;

                    //send save to player
                    if (s[0] == '{') //convert old format to new format
                    {
                        System.IO.File.WriteAllText(
                    dataRoot + "\\Saves\\" + c + ".tusav", "" + utcTime + "\n" + s.Replace("\n", ""));
                        return s;
                    }
                    else
                    {
                        System.IO.File.WriteAllText(
                    dataRoot + "\\Saves\\" + c + ".tusav", "" + utcTime + "\n" + saveParts[1]);
                        return saveParts[1];
                    }
                } else
                {
                    return null;
                }
            }
        }

        //Test if signalR is working
        public Task Ping(string message)
        {
            return Clients.Client(Context.ConnectionId).Pong("FROM SERVER: " + message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ValidityMap.CurrentInstance.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}