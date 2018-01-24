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
        public static ShopList InitializeShop()
        {
            List<Item> itemList = new List<Item>();
            List<Upgrade> upgradeList = new List<Upgrade>();


            //TODO::get details from database


            return new ShopList(itemList, upgradeList);
        }
    }

    public struct ShopList
    {
        public List<Item> itemList;
        public List<Upgrade> upgradeList;

        public ShopList(List<Item> itemList, List<Upgrade> upgradeList)
        {
            this.itemList = itemList;
            this.upgradeList = upgradeList;
        }
    }

    public struct Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BaseCost { get; set; }
        public decimal BaseTps { get; set; }
        public string Icon { get; set; }
        public Item(int id, string name, string description,
            decimal baseCost, decimal baseTps, string icon)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.BaseCost = baseCost;
            this.BaseTps = baseTps;
            this.Icon = icon;
        }

        public void ApplyUpgrade(Upgrade u)
        {
            switch(u.Effect.Item1)
            {
                case "tps":
                    if(u.Effect.Item2.Length == 1)
                    {
                        switch(u.Effect.Item2[0])
                        {
                        }
                    }
                    break;
                case "cost":
                    break;
                case "description":
                    break;
                case "icon":
                    break;
                case "name":
                    break;
            }
        }
    }

    
    public struct Upgrade
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; } //for identification in savefiles
        public string Affected { get; set; }
        public Tuple<string, string, float> Effect { get; set; }
        public string Icon { get; set; }

        public Upgrade(string name, string description, string icon,
            int id, string affected, Tuple<string, string, float> effect)
        {
            this.Name = name;
            this.Description = description;
            this.Id = id;
            this.Affected = affected;
            this.Effect = effect;
            this.Icon = icon;
        }
    }
}