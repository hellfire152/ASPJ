/*
 * Class handling save file related functions
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{ 
    public class SaveFile
    {
        //sends the json as a string
        public static string GetSave(string username)
        {
            string s = System.IO.File.ReadAllText(username + ".tusav");
            return s;
        }

        //takes in player data and serializes it to json, and saves
        public static Boolean Save(string username, Player player)
        {
            return true;
        }
    }
}