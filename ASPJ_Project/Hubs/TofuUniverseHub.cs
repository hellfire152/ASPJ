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
            if (ValidityMap.CurrentInstance[Context.ConnectionId] == false)
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
            if (d.OpenConnection())
            {
                string query = "SELECT * FROM savetime WHERE userID=@id";
                MySqlCommand m = new MySqlCommand(query, d.conn);
                m.Parameters.AddWithValue("@id", c);

                bool hasEntry = false;
                using (MySqlDataReader r = m.ExecuteReader())
                {
                    hasEntry = true;
                    r.Read();
                    times[0] = r.GetInt64("time1");
                    times[1] = r.GetInt64("time2");
                    times[2] = r.GetInt64("time3");
                }
                if(!hasEntry)
                {
                    query = "INSERT INTO savetime (userID, time1, time2, time3) VALUES (@u, @t1, @t2, @t3)";
                    MySqlCommand m2 = new MySqlCommand(query, d.conn);
                    m.Parameters.AddWithValue("@u", c);
                    m.Parameters.AddWithValue("@t1", 0);
                    m.Parameters.AddWithValue("@t2", 0);
                    m.Parameters.AddWithValue("@t3", utcTime);
                    m.ExecuteNonQuery();
                    times = new long[] { 0, 0, utcTime};
                }
                if (utcTime - times[0] < 60000) //4th save in a minute
                {
                    return 0;
                } else
                {
                    times[0] = utcTime;
                    Array.Sort(times);
                    query = "UPDATE savetimes SET time1 = @t1, time2 = @t2, time3 = @t3 WHERE userID = @u";
                    MySqlCommand m3 = new MySqlCommand(query, d.conn);
                    m3.Parameters.AddWithValue("@u", c);
                    m3.Parameters.AddWithValue("@t1", times[0]);
                    m3.Parameters.AddWithValue("@t2", times[1]);
                    m3.Parameters.AddWithValue("@t3", times[2]);
                }
                d.CloseConnection();
            }
            else return -2;

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
                if(d.OpenConnection())
                {
                    string query = @"INSERT INTO cheatlog (username, time) VALUES (@username, @time)";
                    MySqlCommand m = new MySqlCommand(query, d.conn);
                    m.Parameters.AddWithValue("@username", c);
                    m.Parameters.AddWithValue("@time", utcTime);
                    m.ExecuteNonQuery();
                    d.CloseConnection();
                }
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