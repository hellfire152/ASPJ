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
            //read cookie
            string c = Crypto.CurrentInstance.Decrypt(
                Context.RequestCookies["username"].Value);
            Debug.WriteLine("SAVING FOR " + c +":\n" + save.ToString());
            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            System.IO.File.WriteAllText(
                dataRoot + "\\Saves\\" + c + ".tusav", save.ToString());
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
                return System.IO.File.ReadAllText(
                    dataRoot + "\\Saves\\" + c + ".tusav");
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