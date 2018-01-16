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
        //sends the json as a string
        public static string GetSave(string username)
        {
            short mode = (short)MODE.NULL;
            foreach(string line in saveFileLines)
            {   
                //matches [<something>]
                if(line[0] == '[' && line[line.Length - 1] == ']') {
                    //remove the square brackets
                    line.Trim(new char[] { '[', ']' });

                } else
                {

                }
            }

            //TEMP surpress return type error
            return new SaveFile();
        }

        //takes in player data and serializes it to json, and saves
        public static Boolean Save(string username, Player player)
        {
            return true;
        }
    }
}