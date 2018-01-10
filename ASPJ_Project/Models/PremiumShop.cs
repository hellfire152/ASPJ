using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace ASPJ_Project.Models
{
    public class PremiumShop
    {

        public struct PremiumItem
        {
            public string itemName;
            public string itemType;
            public Image itemImage;
            public string itemDescription;
            public double beansPrice;
        }

        public struct User //Dummy User
        {
            public string username;
            public double beansAmount;
            public string creditCardNo;
        }

        //Credit Card Checker
        public static bool checkCard(string creditCardNumber)
        {
            //// check whether input string is null or empty
            if (string.IsNullOrEmpty(creditCardNumber))
            {
                return false;
            }
            //Luhn algorithm
            int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                            .Reverse()
                            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                            .Sum((e) => e / 10 + e % 10);

            //// If the final sum is divisible by 10, then the credit card number
            //   is valid. If it is not divisible by 10, the number is invalid.
            return sumOfDigits % 10 == 0;

        }
        //Check if EXPIRED
        public static bool isValid(string dateString)
        {
            DateTime dateValue;

            if (DateTime.TryParse(dateString, out dateValue))
                if (dateValue < DateTime.Now)
                    return false;
                else
                    return true;
            else
                return false;
        }
    }
}