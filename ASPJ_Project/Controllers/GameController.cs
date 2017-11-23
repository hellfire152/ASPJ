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
        // GET: Game
        public ActionResult Index()
        {
            //HARDCODED SHOP
            List<Item > itemList= new List<Item>();
            List<Upgrade> upgradeList = new List<Upgrade>();

            //init items
            Item cursor = new Item(1, "Cursor", "A cursor for clicking", 10, 0.1m, "Cursor.png");
            Item farm = new Item(2, "Farm", "A farm for farming...?", 500, 10m, "Farm.png");

            //init upgrades
            Upgrade betterClicks = new Upgrade
            {
                Id = 1,
                Name = "Better Clicks",
                Description = "Try harder when clicking",
                Affected = "Click",
                Effect = new Tuple<string, string, float>("cost", "+", 1)
            };

            //add to dictionaries
            itemList.Add(cursor);
            itemList.Add(farm);
            upgradeList.Add(betterClicks);

            ShopList s = new ShopList(itemList, upgradeList);
            return View(s);
        }
    }
}