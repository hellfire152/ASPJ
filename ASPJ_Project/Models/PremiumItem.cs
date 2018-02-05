using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using PayPal.Api;
using System.ComponentModel.DataAnnotations;

namespace ASPJ_Project.Models
{ 

    public class PremiumItem
        {
        public string itemName { get; set; }
        public string itemType { get; set; }
        public string itemID { get; set; }
        public string itemImage { get; set; }
        public string itemDescription { get; set; }
        public int beansPrice { get; set; }
        public int beanAmount { get; set; }
        public string beanIcon { get; set; }
        public double priceOfBeans { get; set; }
    }

}