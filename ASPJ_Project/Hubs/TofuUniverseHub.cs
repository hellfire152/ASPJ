using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ASPJ_Project.Models;

namespace ASPJ_Project.TofuUniverse
{
    public class TofuUniverseHub : Hub
    {
        public static Dictionary<string, Boolean> Validity = new Dictionary<string, bool>();

        public override Task OnConnected()
        {
            //get username from cookie
            var username = Context.RequestCookies["username"].Value;

            //add username to map and set validity
            Validity[username] = UserConnectionMap.CurrentInstance.Add(
                username, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Validity.Remove(UserConnectionMap.CurrentInstance.UsernameOf(
                Context.ConnectionId));
            return base.OnDisconnected(stopCalled);
        }
    }
}