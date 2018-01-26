﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using Newtonsoft.Json;
using ASPJ_Project.Models;

namespace ASPJ_Project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //get keys used for encryption
            //Console.Write("Enter the COOKIE_KEY: ");
            //string cookieKey = Console.ReadLine();

            //initialize upgrades from file
            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string rawUpgrades = System.IO.File.ReadAllText(dataRoot + @"\tofu-universe-upgrades.js");
            string rawItems = System.IO.File.ReadAllText(dataRoot + @"\tofu-universe-items.js");

            //initialize all of our custom classes
            Database.Initialize(System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString());
            Upgrade.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawUpgrades));
            Item.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawItems));
            
            //transfer storage objects to .js files served to the client


            //initialize username to connection map
            Models.UserConnectionMap.CurrentInstance =
                new Models.UserConnectionMap();
            Models.Crypto.CurrentInstance = new Models.Crypto("cookiekey");
        }

    }
}


