using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.ViewModels;
using System.Text;
using System.Web.Util;

namespace ASPJ_Project.Controllers
{
    public class ShopController : Controller
    {

        // GET: Shop
        public ActionResult Shop()
        {
            var shopItems = new List<PremiumShop.PremiumItem>
            {
                new PremiumShop.PremiumItem{ itemName= "Fedora Hat", itemType= "Hat", itemDescription= "Mi'lady.", beansPrice= 100},
                new PremiumShop.PremiumItem{ itemName= "Karate Headband", itemType= "Hat", itemDescription= "Hiya!", beansPrice= 75}
            };

            var currentUser = new PremiumShop.User { username = "jhn905", beansAmount = 400 };

            return View();
        }
        public ActionResult BeansPurchase()
        {
            return View();
        }
        public ActionResult PurchaseConfirmation(string username, int beansAmount, double price)
        {
            ViewData["username"] = username;
            ViewData["beansAmount"] = beansAmount;
            ViewData["price"] = price;
            return View();
        }
    }
}