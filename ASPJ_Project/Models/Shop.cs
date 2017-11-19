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

namespace ASPJ_Project.Models
{
    //initializes a list of all items and upgrades
    //can be done through reading a file or hardcoded here
    public static class Shop
    {
        public static Item cursor = new Item();
    }

    public struct Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public ulong baseCost { get; }
        public ulong baseTps { get; }

        public Item(string name, string description,
            ulong baseCost, ulong baseTps)
        {
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
        public string id { get; } //for identification in savefiles
        public string effect { get;}
    }
}