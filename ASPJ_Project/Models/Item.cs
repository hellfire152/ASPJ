using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Item
    {
        public static Dictionary<int, dynamic> AllItems = new Dictionary<int, dynamic>();

        public static void Initialize(Dictionary<int, dynamic> itemJson)
        {
            foreach(var itemId in itemJson.Keys)
            {
                //add item's base cost and base tps to dictionary
                var i = itemJson[itemId];
                if (i.cost == null) i.cost = 0;
                AllItems.Add(itemId,
                    new Item(itemId, (decimal)i.tps, (decimal)i.cost));
            }
        }

        public decimal Tps { get; set; }
        public decimal Cost { get; set; }
        public int Id { get; }

        public Item(int id, decimal tps, decimal cost)
        {
            this.Id = id;
            this.Tps = tps;
            this.Cost = cost;
        }
    }
}