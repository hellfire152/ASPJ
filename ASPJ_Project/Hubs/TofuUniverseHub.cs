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
            if (!ValidityMap.CurrentInstance.Contains(Context.ConnectionId) || 
                !ValidityMap.CurrentInstance[Context.ConnectionId])
            {
                Debug.WriteLine("INVALID CONNECTION");
                return -1;
            }


            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            //read cookie
            string c = AESCryptoStuff.CurrentInstance.AesDecrypt(HttpUtility.UrlDecode(Context.RequestCookies["userID"].Value));

            //current time in UTC
            long utcTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Hmac h = Hmac.CurrentInstance;

            #region SaveTime limit to 3 / min
            //check database if it's too soon
            Database d = Database.CurrentInstance; long[] times = new long[3];
            //PRQ stands for Parameterized Reader Query, it returns a DataTable with all the rows
            //First argument is the query, every argument after that is the parameters
            //The @ parameters MUST START FROM 1 COUNTS UP FROM THERE
            //you can have any number of @ parameters and corresponding method arguments for the values
            DataTable dt = d.PRQ("SELECT * FROM savetime WHERE userID = @1", c);
            if (dt == null) return -2; //if database not up
            if(dt.Rows.Count > 0)
            {
                //if you want to loop
                //foreach(DataRow dr in dt.Rows)
                DataRow dr = dt.Rows[0];
                //Field method returns the value of the column specified in the type in the angle brackets
                times[0] = dr.Field<long>("time1");
                times[1] = dr.Field<long>("time2");
                times[2] = dr.Field<long>("time3");
            }
            else
            {
                //PNQ stands for Parameterized Non Query, it returns nothing
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
                    times[0], times[1], times[2], c);
            }
            #endregion

            //get previous save data
            SaveFile prevSave;
            try
            {
                string prevSaveText = System.IO.File.ReadAllText(
                        dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav");
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
                d.PNQ("INSERT INTO cheatlog (userID, time) VALUES (@1, @2)", c, utcTime);
                ValidityMap.CurrentInstance[Context.ConnectionId] = false;
                return -1;
            }

            //save + time on first line
            string s = "" + utcTime
                + '\n' + progress.ToString();
            Debug.WriteLine("SAVING FOR " + h.Encode(c) +":\n" + s);

            //write to file
            System.IO.File.WriteAllText(
                dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav", s);

            return 1;
        }

        //gets a save file and sends it to the client
        public string RequestSave(string code)
        {
            Hmac h = Hmac.CurrentInstance;
            //read cookie
            Debug.WriteLine(Context.RequestCookies["userID"].Value);
            string c = HttpUtility.UrlDecode(AESCryptoStuff.CurrentInstance.AesDecrypt(Context.RequestCookies["userID"].Value));

            #region Check access code
            DataTable dt = Database.CurrentInstance.PRQ(
                "SELECT code FROM saveaccess WHERE userID = @1", c);
            if (dt.Rows.Count == 0) return "invalid:No access code";
            bool validCode = false; code = HttpUtility.HtmlDecode(code);
            foreach(DataRow r in dt.Rows)
            {
                if (r.Field<string>("code") == code) validCode = true;
            }
            if (!validCode) return "invalid:Wrong access code";
            Database.CurrentInstance.PNQ(
                "DELETE FROM saveaccess WHERE userID = @1", c);
            ValidityMap.CurrentInstance.Add(Context.ConnectionId, true);
            #endregion

            if (c == null || c == "guest")
            {
                return "invalid:No username attached";
            } else
            {
                Debug.Write("GETTING SAVE FILE OF: " + c);
                string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Debug.WriteLine(dataRoot);
                string saveFileLocation = dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav";
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
                    dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav", "" + utcTime + "\n" + s.Replace("\n", ""));
                        return s;
                    }
                    else
                    {
                        System.IO.File.WriteAllText(
                    dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav", "" + utcTime + "\n" + saveParts[1]);
                        return saveParts[1];
                    }
                } else
                {
                    return null;
                }
            }
        }

        //ends the game
        public int Collapse()
        {
            //if connection is already invalidated
            if (!ValidityMap.CurrentInstance.Contains(Context.ConnectionId) ||
                !ValidityMap.CurrentInstance[Context.ConnectionId])
            {
                Debug.WriteLine("INVALID CONNECTION");
                return -2;
            }

            Hmac h = Hmac.CurrentInstance;
            //read cookie
            string c = HttpUtility.UrlDecode(AESCryptoStuff.CurrentInstance.AesDecrypt(Context.RequestCookies["userID"].Value));
            if (c == null || c == "guest") //you have to be logged in to collapse XD
            {
                return -2;
            }
            else
            {
                string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                string saveFileLocation = dataRoot + "\\Saves\\" + h.Encode(c) + ".tusav";
                if (File.Exists(saveFileLocation))
                {
                    //get savefile
                    string s = System.IO.File.ReadAllText(saveFileLocation);

                    SaveFile save = SaveFile.Parse(s);
                    //need 1 quadrillion tofu to pass
                    if (save.TCount >= 1000000000000000000)
                    {
                        //delete save file
                        File.Delete(saveFileLocation);
                        return 1;
                    }
                    else return 0;
                }
                else
                {
                    return -1;
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