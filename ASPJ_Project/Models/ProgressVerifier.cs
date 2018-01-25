using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class ProgressVerifier
    {
        public static Boolean VerifyProgress(
            SaveFile save, ProgressData progress, long currentUtcTime)
        {
            //calculate time passed in seconds
            long timePassed = (currentUtcTime - save.Time) / 1000;

            //apply upgrades

            return true;
        }
    }

    public class ItemData
    {
        public Dictionary<int, Item> Data;
        public ItemData()
        {
            //initialize and clone static Items store
            if (Item.AllItems.Keys.Count <= 0) throw new Exception("Item object not initialized!");
            this.Data = new Dictionary<int, Item>();
            foreach (Item item in Item.AllItems.Values)
            {
                this.Data.Add(item.Id,
                    new Item(item.Id, item.Tps, item.Cost));
            };
        }

        //calculates the tCount needed to make the purchase
        public double Purchase(int itemId, int alreadyPurchased, int currentlyOwned)
        {
            Item i = Data[itemId];
            double baseCost = i.Cost;
            
            double cost = System.Math.Round(baseCost * Math.Pow(1.15, alreadyPurchased));

            int bought = currentlyOwned - alreadyPurchased; double totalCost = 0;
            for(int j = 0; j < bought; j++)
            {
                totalCost += cost;
                cost = System.Math.Round(baseCost * Math.Pow(1.15, ++alreadyPurchased));
            }

            return totalCost;
        }
    }
}