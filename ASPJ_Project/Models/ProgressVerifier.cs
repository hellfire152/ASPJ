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

    class ItemData
    {
        public Dictionary<int, Item> Data;
        public ItemData()
        {
            //initialize and clone static Items store
            this.Data = new Dictionary<int, Item>();
            foreach (Item item in Item.AllItems.Values)
            {
                this.Data.Add(item.Id,
                    new Item(item.Id, item.Tps, item.Cost));
            };
        }

        public void Purchase(int itemId, int alreadyPurchased, int noPurchased)
        {
            
        }
    }
}