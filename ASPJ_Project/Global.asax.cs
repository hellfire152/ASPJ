using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;
using ASPJ_Project.Models;
using System.IO;
using System.Web.Configuration;
using System.Diagnostics;

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
            MvcHandler.DisableMvcResponseHeader = true; //to remove MVC version disclosure

            //getting all the data required for initialization
            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string rawUpgrades = File.ReadAllText(dataRoot + @"\tofu-universe-upgrades.js");
            string rawItems = File.ReadAllText(dataRoot + @"\tofu-universe-items.js");
            string iv = WebConfigurationManager.AppSettings["iv"];
            string key = WebConfigurationManager.AppSettings["key"];

            //initialize all of our custom classes
            Database.Initialize(System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString());
            Upgrade.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawUpgrades));
            Item.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawItems));
            AESCryptoStuff.Initialize(iv, key);
            Hmac.Initialize(key);
            ValidityMap.Initialize();

            //transfer storage objects to .js files served to the client
            string scriptsRoot = Server.MapPath("~") + @"\Scripts\TofuUniverse\";
            string itemsFile = "let _tofuUniverse = {}; _tofuUniverse.ITEMS = " + rawItems;
            string upgradesFile = "_tofuUniverse.UPGRADES = " + rawUpgrades;
            File.WriteAllText(scriptsRoot + "tofu-universe-items.js", itemsFile);
            File.WriteAllText(scriptsRoot + "tofu-universe-upgrades.js", upgradesFile);

        }

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
            //NOTE: Stopping IE from caching
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            // Stop Caching in Firefox
            HttpContext.Current.Response.Cache.SetNoStore();
            //Prevent other websites from iframing except for origin
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
            //Enables XSS filtering, sanitizes page
            HttpContext.Current.Response.AddHeader("X-XSS-Protection", "1");
            HttpContext.Current.Response.AddHeader("X-Content-Type-Options", "nosniff");
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetValidUntilExpires(true);
        }

    }
}


