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
            //Session["userID"] = 49;
            //not logged in
            if ((int)Session["userID"] == 0) return RedirectToAction("Login", "User");

            //set username cookie
            HttpCookie usernameCookie = new HttpCookie("userID")
            {
                Value = HttpUtility.UrlEncode(AESCryptoStuff.CurrentInstance.AesEncrypt(""+Session["userID"]))
            };
            Response.SetCookie(usernameCookie);

            #region Equip Items
            Database d = Database.CurrentInstance;
            string query =
                @"SELECT e.userID, h.ItemImage AS hatImage, o.itemImage AS outfitImage
                  FROM equippeditems AS e
                  LEFT OUTER JOIN premiumitem AS h
                  ON e.equippedHat = h.itemID
                  LEFT OUTER JOIN premiumitem AS o
                  ON e.equippedOutfit = o.itemID
                  WHERE userID = @1";
            DataTable dt = d.PRQ(query, Session["userID"]);
            if(dt.Rows.Count != 0)
            {
                string hatImage = dt.Rows[0].Field<string>("hatImage");
                string outfitImage = dt.Rows[0].Field<string>("outfitImage");

                ViewData["hat"] = hatImage;
                ViewData["outfit"] = outfitImage;
            }
            #endregion

            #region Access Code
            string code = System.Web.Security.Membership.GeneratePassword(128, 25);
            ViewData["code"] = code;
            Database.CurrentInstance.PNQ("INSERT INTO saveaccess (userID, code) VALUES (@1, @2)",
                Session["userID"], code);
            #endregion

            return View();
        }
    }
}