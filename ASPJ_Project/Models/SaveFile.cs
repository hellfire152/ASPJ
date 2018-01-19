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

namespace ASPJ_Project.Models
{
    
    public class SaveFile
    {
        //takes in player data and serializes it to json, and saves
        public static Boolean Save(string username, Player player)
        {
            return true;
        }
    }
}