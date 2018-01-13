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

        public Boolean SaveProgress(string save)
        {
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);
            System.IO.File.WriteAllText("..\\Saves\\" + c + ".tusav", save);
            return true;
        }

        public string RequestSave()
        {
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);
            if (c == null || c == "guest")
            {
                return "NA";
            } else
            {
                Debug.Write("GETTING SAVE FILE OF: " + c);
                string projectRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                Debug.WriteLine(projectRoot);
                return System.IO.File.ReadAllText(
                    projectRoot + "\\Saves\\" + c + ".tusav");
            }
        }

        //test for set username
        public string RequestUsername()
        {
            return Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);
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
        }
        */
    }
}