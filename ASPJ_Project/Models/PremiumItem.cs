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
            public string itemName;
            public string itemType;
            public string itemID;
            public string itemImage;
            public string itemDescription;
            public int beansPrice;
            public int beanAmount;
            public string beanIcon;
        }

}