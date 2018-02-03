using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class EquippedItem
    {
        public int userID { get; set; }
        public int equippedHat { get; set; }
        public int equippedOutfit { get; set; }
    }
}