using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TofuAuthorizeAttribute : AuthorizeAttribute
    {

        public override bool AuthorizeHubConnection(
            Microsoft.AspNet.SignalR.Hubs.HubDescriptor hubDescriptor, IRequest request)
        {
            if (UserConnectionMap.CurrentInstance.ConnectionOf(
                Crypto.CurrentInstance.Decrypt(
                    request.Cookies["username"].Value)) != null)
            {
                return false;
            }
            else return true;
        }
    }
}