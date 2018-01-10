using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;

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

            //initialize username to connection map
            Models.UserConnectionMap.CurrentInstance =
                new Models.UserConnectionMap();
            Models.Crypto.CurrentInstance = new Models.Crypto("cookiekey");
        }
    }

    public class DubuBaseConnect 
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        String queryStr;

        private void DubuInit()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            Console.Write("Connection Success");
            queryStr = " ";

            /*queryStr = "INSERT INTO dububase.accounts (userName, password, email, phoneNumber)" + 
                "VALUES"(firstname.Text + lastname.Text + "," + phoneNumber.Text)";*/

            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

            cmd.ExecuteReader();

            conn.Close();
        }
    }
}


