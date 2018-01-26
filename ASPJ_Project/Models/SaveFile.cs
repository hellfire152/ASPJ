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
        public double TCount { get; }
        public Dictionary<string, int> Items { get; }
        public int[] Upgrades { get; }
        public long Time { get; set; }

        public SaveFile(long time, double tCount, 
            Dictionary<string, int> items, int[] upgrades)
        {
            this.Time = time;
            this.TCount = tCount;
            this.Items = items;
            this.Upgrades = upgrades;
        }

        //takes in the raw save string and returns a SaveFile object
        public static SaveFile Parse(string saveString)
        {
            SaveFile save; long t;
            if(saveString[0] != '{')
            {
                //get time
                string[] timeSplit = saveString.Split('\n');
                long.TryParse(timeSplit[0], out t);
                save = JsonConvert.DeserializeObject<SaveFile>(timeSplit[1]);
                save.Time = t;
            } else //no time available
            {
                save = JsonConvert.DeserializeObject<SaveFile>(saveString);
                t = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
                save.Time = t;
            }

            //check for invalid save
            if (t == 0 || save.Items == null
                || save.Upgrades == null) return null;

            return save;
        }
    }
}