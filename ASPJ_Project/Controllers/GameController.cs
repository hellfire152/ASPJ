using ASPJ_Project.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASPJ_Project.Controllers
{
    public class GameController : Controller
    {
        //needed to set test accounts
        public static string[] TestUsernames = {"1", "2", "3", "4"};
        public static int TestCounter = 0;

        [HttpGet]
        public ActionResult Index()
        {
            //set test username
            HttpCookie usernameCookie = new HttpCookie("username")
            {
                Value = TestUsernames[TestCounter]
            };
            //comment out this line to cycle between 1,2,3,4
            usernameCookie.Value = "test2";
            if (++TestCounter > 3) TestCounter = 0;

            Response.SetCookie(usernameCookie);

            //generate one-time key
            string oneTimePass = Membership.GeneratePassword(128, 20);
            //store key in database
            Database d = Database.CurrentInstance;
            if(d.OpenConnection())
            {
                string query = "INSERT INTO gameaccess (username, accesskey) VALUES (@username, @key)";
                MySqlCommand m = new MySqlCommand(query, d.conn);
                m.Parameters.AddWithValue("@username", usernameCookie.Value);
                m.Parameters.AddWithValue("@key", oneTimePass);
                m.ExecuteNonQuery();
            }
            ViewData["accessCode"] = oneTimePass;
            return View();
        }
    }
}