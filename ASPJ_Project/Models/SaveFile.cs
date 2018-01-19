/*
 * Class used to parse the save file
 * Save files are stored in this format:
 * 
 * <bean count>
 * <tofu count>
 * <tofu per second>
 * <list of comma-separated numbers, representing items>
 * <same as above, for upgrades>
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace ASPJ_Project.Models
{
    
    public class SaveFile
    {
        private enum ItemIds : int
        {
            Cursor = 1,
            Farm = 2,
            Test = 3
        }
        public double TCount { get; }
        public Dictionary<int, int> Items { get; }
        public int[] Upgrades { get; }
        public long Time { get; set; }

        public SaveFile(long time, double tCount, 
            Dictionary<int, int> items, int[] upgrades)
        {
            this.Time = time;
            this.TCount = tCount;
            this.Items = items;
            this.Upgrades = upgrades;
        }

        //takes in the raw save string and returns a SaveFile object
        public static SaveFile Parse(string saveString)
        {
            //get time
            string[] timeSplit = saveString.Split('\n');
            long.TryParse(timeSplit[0], out long t);
            SaveFile save = JsonConvert.DeserializeObject<SaveFile>(timeSplit[1]);
            save.Time = t;

            //check for invalid save
            if (t == 0 || save.Items == null
                || save.Upgrades == null) return null;
            return null;
        }
    }
}