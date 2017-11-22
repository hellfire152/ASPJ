/*
 * Class of constants, used to store information regarding  
 * everything purchasable in the shop
 * 
 * Also includes a struct detailing how shop items are stored
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ASPJ_Project.Models
{
    //initializes a list of all items and upgrades
    //can be done through reading a file or hardcoded here
    public static class Shop
    {
        public static ShopList initializeShop()
        {
            Dictionary<int, Item> itemList = new Dictionary<int,Item>();
            Dictionary<int, Upgrade> upgradeList = new Dictionary<int, Upgrade>();


            //TODO::get details from database


            return new ShopList(itemList, upgradeList);
        }
    }

    public struct ShopList
    {
        public Dictionary<int, Item> itemList;
        public Dictionary<int, Upgrade> upgradeList;

        public ShopList(Dictionary<int, Item> itemList, Dictionary<int, Upgrade> upgradeList)
        {
            this.itemList = itemList;
            this.upgradeList = upgradeList;
        }
    }

    public struct Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal baseCost { get; set; }
        public decimal baseTps { get; set; }

        public Item(int id, string name, string description,
            decimal baseCost, decimal baseTps)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.baseCost = baseCost;
            this.baseTps = baseTps;
        }
    }

    
    public struct Upgrade
    {
        public string name { get; set; }
        public string description { get; set; }
        public int id { get; set; } //for identification in savefiles
        public string affected { get; set; }
        public Tuple<char, float> effect { get; set; }

        public Upgrade(string name, string description,
            int id, string affected, Tuple<char, float> effect)
        {
            this.name = name;
            this.description = description;
            this.id = id;
            this.affected = affected;
            this.effect = effect;
        }
    }
}