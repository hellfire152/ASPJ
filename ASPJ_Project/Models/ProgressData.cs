using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class ProgressData
    {
        public double TCount;
        public Dictionary<int, int> Items;
        public int[] Upgrades;
        public int tofuClicks;

        public override string ToString()
        {
            string s = "{\"tCount\":" + TCount + ",\"items\":{";
            string[] itemKVPairs = new string[Items.Count - 1];
            int j = 0;
            foreach(KeyValuePair<int, int> i in Items)
            {
                if (i.Key != 0)
                    itemKVPairs[j++] = "\"" + i.Key + "\":" + i.Value;
            }
            s = s + String.Join(",", itemKVPairs) + "},\"upgrades\":[";
            s = s + String.Join(",", Upgrades) + "]}";
            return s;
        }
    }
}