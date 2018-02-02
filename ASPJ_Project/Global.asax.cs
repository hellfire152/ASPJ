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

            //initialize upgrades from file
            string dataRoot = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string rawUpgrades = File.ReadAllText(dataRoot + @"\tofu-universe-upgrades.js");
            string rawItems = File.ReadAllText(dataRoot + @"\tofu-universe-items.js");

            //initialize all of our custom classes
            Database.Initialize(System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString());
            Upgrade.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawUpgrades));
            Item.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(rawItems));
            AESCryptoStuff.Initialize(iv, key);
            ValidityMap.Initialize();

            //transfer storage objects to .js files served to the client
            string scriptsRoot = Server.MapPath("~") + @"\Scripts\TofuUniverse\";
            string itemsFile = "let _tofuUniverse = {}; _tofuUniverse.ITEMS = " + rawItems;
            string upgradesFile = "_tofuUniverse.UPGRADES = " + rawUpgrades;
            File.WriteAllText(scriptsRoot + "tofu-universe-items.js", itemsFile);
            File.WriteAllText(scriptsRoot + "tofu-universe-upgrades.js", upgradesFile);

            //initialize username to connection map
            Models.UserConnectionMap.CurrentInstance =
                new Models.UserConnectionMap();
            Models.Crypto.CurrentInstance = new Models.Crypto("cookiekey");

        }

    }
}


