﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (timePassed <= 0) timePassed = 1;

            //get all upgrades of save
            Upgrade[] upgradeArr = new Upgrade[save.Upgrades.Length];
            if(upgradeArr.Length != 0)
            {
                for(int i = 0; i < save.Upgrades.Length; i++)
                {
                    upgradeArr[i] = Upgrade.upgradeData[save.Upgrades[i]];
                }
            }

            //apply upgrades of save
            ItemData itemData = new ItemData();
            List<Effect> allEffects = GetAllEffects(upgradeArr);
            SortEffects(allEffects);
            ApplyEffects(allEffects, itemData);

            //tps before any purchases
            decimal currentTps = RecalculateTps(itemData, save.Items);
            long lowerTimeBound = save.Time; long initialTime = save.Time;
            decimal tofuCount = save.TCount;
            //calculates actual tofuCount + click leeway
            //also does cost checking, and adjusts tps for every purchase
            foreach(Tuple<long, string, int> purchase in progress.purchases)
            {
                Boolean isItem = purchase.Item2 == "item";
                //generate tofu
                tofuCount += CalculateTofuGenerated(
                    initialTime, lowerTimeBound, purchase.Item1, currentTps, itemData);

                //pay cost
                if (isItem) tofuCount -= itemData.Data[purchase.Item3].Cost;
                else tofuCount -= Upgrade.upgradeData[purchase.Item3].cost;
                Debug.WriteLine("TCOUNT AFTER PAYING COST: " + tofuCount);
                if (tofuCount < 0) return false; //cannot afford purchase

                //calculate resulting tps
                if (isItem)
                {
                    save.Items[purchase.Item3]++;
                } else
                {
                    allEffects.AddRange(
                        GetEffects(Upgrade.upgradeData[purchase.Item3]));
                    itemData = new ItemData();
                    SortEffects(allEffects);
                    ApplyEffects(allEffects, itemData);
                }
                currentTps = RecalculateTps(itemData, save.Items);

                lowerTimeBound = purchase.Item1;
            }
            //after final purchase to time of save
            long finalTimeDiff = (currentUtcTime - lowerTimeBound) / 1000;
            tofuCount += CalculateTofuGenerated(
                initialTime, lowerTimeBound, currentUtcTime, currentTps, itemData);

            //returns false if client tCount is more than expected
            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine("EXPECTED: <" + tofuCount + "\nACTUAL: " + progress.TCount);
            if (progress.TCount > tofuCount)
            {
                Debug.WriteLine("FINAL TCOUNT INVALID");
                return false;
            }
            return true;
        }

        public static decimal CalculateTofuGenerated(
            long initialTime, long startTime, long endTime, decimal currentTps, ItemData i)
        {
            decimal tofuGenerated = 0;
            long timeDiff = (endTime - startTime) / 1000;
            Debug.WriteLine("SECONDS PASSED: " + timeDiff);
            //calculate tofu owned at moment of purchase
            //auto-generated tofu
            tofuGenerated += timeDiff * currentTps;
            Debug.WriteLine("NO. OF TOFU AUTO-GENERATED: " + tofuGenerated);
            //tofu clicks
            long lowerBound = (startTime - initialTime) / 1000;
            long upperBound = (endTime - initialTime) / 1000;
            tofuGenerated += CalculateClickLeeway(
                lowerBound, upperBound, i.tofuClickEarnings);
            Debug.WriteLine("AFTER CLICK LEEWAY: " + tofuGenerated);
            return tofuGenerated;
        }

        public static decimal RecalculateTps(ItemData items, Dictionary<int, int> nowOwned)
        {
            decimal totalTps = 0;
            foreach (Item i in items.Data.Values)
            {
                if (i.Id == 0) continue; //manually skip id 0 (reserved)
                totalTps += i.Tps * nowOwned[i.Id];
            }
            return totalTps;
        }

        public static decimal CalculateClickLeeway(long startTime, long endTime, decimal tofuPerClick)
        {
            double tn = (510 * Math.Log(startTime + 40)) + (2 * startTime);
            double tnplus1 = (510 * Math.Log(endTime + 40)) + (2 * endTime);
            decimal integralResult = Convert.ToDecimal(tnplus1 - tn);
            return tofuPerClick * integralResult;
        }

        public static List<Effect> GetAllEffects(Upgrade[] upgrades)
        {
            List<Effect> allEffects = new List<Effect>();
            foreach(Upgrade u in upgrades)
            {
                allEffects.AddRange(GetEffects(u));
            }
            return allEffects;
        }

        public static List<Effect> GetEffects(Upgrade upgrade)
        {
            List<Effect> effects = new List<Effect>();
            foreach(Effect e in upgrade.Effects)
            {
                effects.Add(e);
            }
            return effects;
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
        public decimal tofuClickEarnings;
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
        public decimal PurchaseAll(SaveFile save, Dictionary<int, int> nowOwned)
        {
            decimal totalCost = 0;
            foreach(int itemId in Data.Keys)
            {
                nowOwned.TryGetValue(itemId, out int iOwned);
                if(iOwned != 0)
                    totalCost += Purchase(itemId, save.Items[itemId], nowOwned[itemId]);
            }

            return totalCost;
        }

        //calculates the tCount needed to make the purchase
        public decimal Purchase(int itemId, int alreadyPurchased, int currentlyOwned)
        {
            Item i = Data[itemId];
            decimal baseCost = i.Cost;
            
            decimal cost = System.Math.Round(baseCost * Convert.ToDecimal(Math.Pow(1.15, alreadyPurchased)));

            int bought = currentlyOwned - alreadyPurchased; decimal totalCost = 0;
            if (bought <= 0) return 0;
            
            for(int j = 0; j < bought; j++)
            {
                totalCost += cost;
                cost = System.Math.Round(baseCost * Convert.ToDecimal(Math.Pow(1.15, ++alreadyPurchased)));
            }

            return totalCost;
        }
    }
}