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
        //enum of parsing modes
        enum MODE : short { NULL=0, PROFILE=1, GAME=2, ITEMS=3, UPGRADES=4}
        /* Reads a string array (returned from System.IO.File.ReadAllLines)
         * representing the save file, and returns a SaveFile object with
         * all the appropriate data
         */
        public static SaveFile ParseSave(String[] saveFileLines)
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

        private Dictionary<string, string> Profile { get; set; }
        private Dictionary<string, string> Game { get; set; }
        private Dictionary<int, int> Items { get; set; }
        private int[] Upgrades { get; set; }
    }
}