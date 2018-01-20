using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ASPJ_Project.Models;
using System.Diagnostics;

namespace ASPJ_Project.TofuUniverse
{   
    //[TofuAuthorize]
    public class TofuUniverseHub : Hub
    {
        //public static Dictionary<string, Boolean> Validity = new Dictionary<string, bool>();

        public Boolean SaveProgress(ProgressData save)
        {
            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);

            //current time in UTC
            long utcTime = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            //get previous save data
            string prevSaveText = System.IO.File.ReadAllText(
                    dataRoot + "\\Saves\\" + c + ".tusav");
            SaveFile prevSave = SaveFile.Parse(prevSaveText);
            //verify progress
            ProgressVerifier.VerifyProgress(prevSave, save, utcTime);

            //save + time on first line
            string s = "" + utcTime
                + '\n' + save.ToString();
            Debug.WriteLine("SAVING FOR " + c +":\n" + s);

            //write to file
            System.IO.File.WriteAllText(
                dataRoot + "\\Saves\\" + c + ".tusav", s);
            return true;
        }

        public string RequestSave()
        {
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
               Context.RequestCookies["username"].Value);
            if (c == null || c == "guest")
            {
                return null;
            } else
            {
                Debug.Write("GETTING SAVE FILE OF: " + c);
                string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Debug.WriteLine(dataRoot);
                //get savefile
                string s = System.IO.File.ReadAllText(
                    dataRoot + "\\Saves\\" + c + ".tusav");
                //remove the time from save
                return s.Split('\n')[1];
            }
        }

        //test for set username
        public string RequestUsername()
        {
            // return Crypto.CurrentInstance.Decrypt(
            //   Context.RequestCookies["username"].Value);
            string c = (string)HttpContext.Current.Session["username"];
            return c ?? "test";
        }

        //Test if signalR is working
        public Task Ping(string message)
        {
            return Clients.Client(Context.ConnectionId).Pong("FROM SERVER: " + message);
        }

        /*public override Task OnConnected()
        {
            //get username from cookie
            var username = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);
            if(!(username == null || username == "guest")) //logged in user
            {
                //map connection
                UserConnectionMap.CurrentInstance.Add(username, Context.ConnectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserConnectionMap.CurrentInstance.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }*/
    }
}