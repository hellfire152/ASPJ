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

            //check tofu click numbers
            double tofuClicksPerSecond = progress.tofuClicks / timePassed;
            if (tofuClicksPerSecond > 20 || (tofuClicksPerSecond > 10 && timePassed > 60))
                return false;

            //getting base Item stats
            ItemData itemData = new ItemData();

            //get all upgrades
            Upgrade[] upgradeArr = new Upgrade[progress.Upgrades.Length];
            for(int i = 0; i < progress.Upgrades.Length; i++)
            {
                upgradeArr[i] = Upgrade.upgradeData[progress.Upgrades[i]];
            }

            //apply upgrades
            List<Effect> allEffects = GetAllEffects(upgradeArr);
            SortEffects(allEffects);
            ApplyEffects(allEffects, itemData);

            //calculate cost
            double totalCost = 0;
            //of all upgrades
            foreach (Upgrade u in upgradeArr) totalCost += u.cost;
            //of all items
            totalCost += itemData.PurchaseAll(save, progress.Items);

            //calculate tps
            double totalTps = 0;
            foreach(Item i in itemData.Data.Values)
            {
                totalTps += i.Tps * progress.Items[i.Id];
            }

            //calculate tofu earned
            double tofuEarned = 0;
            //via auto-generation
            tofuEarned += totalTps * timePassed;
            //via tofu clicks
            tofuEarned += progress.tofuClicks * itemData.tofuClickEarnings;

            //see if final tofu amount checks out
            if (save.TCount + tofuEarned - totalCost < progress.TCount)
                return false;
            return true;
        }

        public static List<Effect> GetAllEffects(Upgrade[] upgrades)
        {
            List<Effect> allEffects = new List<Effect>();
            foreach(Upgrade u in upgrades)
            {
                foreach(Effect e in u.Effects)
                {
                    allEffects.Add(e);
                }
            }
            return allEffects;
        }

        public static void SortEffects(List<Effect> allEffects)
        {
            allEffects.Sort(delegate (Effect x, Effect y)
            {
                return OperatorRank[x.Operator[0]] - OperatorRank[y.Operator[0]];
            });
        }

        public static void ApplyEffects(List<Effect> allEffects,
           ItemData itemData)
        {
            foreach(Effect e in allEffects)
            {
                Item i = itemData.Data[int.Parse(e.Benefactor)];
                if (e.BenefactorProperty != "tps") continue; //we only care about tps effects
                bool click = e.Benefactor == "0";
                switch(e.Operator)
                {
                    case "*":
                        if (click) itemData.tofuClickEarnings *= e.Operand;
                        else i.Tps *= e.Operand;
                        break;
                    case "+":
                        if (click) itemData.tofuClickEarnings += e.Operand;
                        else i.Tps += e.Operand;
                        break;
                    case "-":
                        if (click) itemData.tofuClickEarnings -= e.Operand;
                        else i.Tps -= e.Operand;
                        break;
                    case "/":
                        if (click) itemData.tofuClickEarnings /= e.Operand;
                        else i.Tps /= e.Operand;
                        break;
                    case "=":
                        if (click) itemData.tofuClickEarnings = e.Operand;
                        else i.Tps = e.Operand;
                        break;
                    default:
                        throw new Exception("Operator not recognized!");
                }
            }
        }

        public static Dictionary<char, int> OperatorRank = new Dictionary<char, int>{
            { '+', 0 },
            { '-', 1 },
            { '*', 2 },
            { '/', 3 },
            { '=', 4 }
        };
    }

    public class ItemData
    {
        public Dictionary<int, Item> Data;
        public double tofuClickEarnings;
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
            this.tofuClickEarnings = 1;
        }

        //calculates tCount needed to make all item purchases
        public double PurchaseAll(SaveFile save, Dictionary<int, int> nowOwned)
        {
            double totalCost = 0;
            foreach(int itemId in Data.Keys)
            {
                nowOwned.TryGetValue(itemId, out int iOwned);
                if(iOwned != 0)
                    totalCost += Purchase(itemId, save.Items[""+itemId], nowOwned[itemId]);
            }

            return totalCost;
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