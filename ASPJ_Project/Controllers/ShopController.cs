using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.ViewModels;

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
            return View();
        }
        public ActionResult PremiumShop()
        {
            return View();
        }
    }
}