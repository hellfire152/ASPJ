using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TofuUniverse
{
    public class TofuUniverseHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}