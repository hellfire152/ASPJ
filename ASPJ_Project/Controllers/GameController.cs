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
            ShopList s = new ShopList();
            Dictionary<int, Item > itemList= new Dictionary<int, Item>();
            Dictionary<int, Upgrade> upgradeList = new Dictionary<int, Upgrade>();

            //init items
            Item cursor = new Item(1, "Cursor", "A cursor for clicking", 10, 0.1m);
            Item farm = new Item(2, "Farm", "A farm for farming...?", 500, 10m);

            //init upgrades
            Upgrade betterClicks = new Upgrade
            {
                id = 1,
                name = "Better Clicks",
                description = "Try harder when clicking",
                affected = "Click",
                effect = new Tuple<char, float>('+', 1)
            };

            //add to dictionaries
            itemList.Add(cursor.id, cursor);
            itemList.Add(farm.id, farm);
            upgradeList.Add(betterClicks.id, betterClicks);

            return View(s);
        }
    }
}