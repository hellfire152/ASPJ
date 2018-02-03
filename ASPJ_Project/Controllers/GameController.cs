using ASPJ_Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPJ_Project.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            //test username
            Session["UserID"] = 49;
            if ((int)Session["UserID"] == 0) return View("Error");

            //set username cookie
            HttpCookie usernameCookie = new HttpCookie("UserID")
            {
                Value = HttpUtility.UrlEncode(AESCryptoStuff.CurrentInstance.AesEncrypt(""+Session["UserID"]))
            };
            Response.SetCookie(usernameCookie);

            string code = System.Web.Security.Membership.GeneratePassword(128, 25);
            ViewData["code"] = code;
            Database.CurrentInstance.PNQ("INSERT INTO saveaccess (userID, code) VALUES (@1, @2)",
                Session["UserID"], code);
            return View();
        }
    }
}