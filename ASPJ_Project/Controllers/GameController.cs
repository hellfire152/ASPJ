using ASPJ_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                Value = Crypto.CurrentInstance.Encrypt(
                    TestUsernames[TestCounter])
            };
            //comment out this line to cycle between 1,2,3,4
            usernameCookie.Value = "1";
            if (++TestCounter > 3) TestCounter = 0;

            Response.SetCookie(usernameCookie);
            return View();
        }
    }
}