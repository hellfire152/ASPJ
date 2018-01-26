using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using PayPal.Api;
using System.ComponentModel.DataAnnotations;

namespace ASPJ_Project.Models
{
    public class PremiumShop
    {

        public struct PremiumItem
        {
            public string itemName;
            public string itemType;
            //public Image itemImage;
            public string itemDescription;
            public double beansPrice;
        }

        public struct User //Dummy User
        {
            public string username;
            public double beansAmount;
        }

        public struct BeanAndPrice
        {
            public string beansName;
            public double beans;
            public double price;
        }

        public struct Address
        {
            public string city { get; set; }
            public string country_code { get; set; }
            public string line1 { get; set; }
            public string line2 { get; set; }
            public string postal_code { get; set; }
            public string state { get; set; }
        }

        public struct CreditCard
        {
            public Address billing_address { get; set; }

            [Required(ErrorMessage = "Credit Card Number is required.")]
            [StringLength(16, ErrorMessage = "Credit card must be 16 digits long.")]
            public string creditCardNo { get; set; }

            [Required(ErrorMessage = "CVV is required.")]
            [StringLength(3, ErrorMessage = "CVV must only be 3 digits long.", MinimumLength =3)]
            public string cvv2 { get; set; }

            public int expire_month { get; set; }
            public int expire_year { get; set; }

            [Required(ErrorMessage = "First name is required.")]
            [StringLength(20)]
            public string first_name { get; set; }

            [Required(ErrorMessage = "Last name is required.")]
            [StringLength(20)]
            public string last_name { get; set; }

            public string type { get; set; }
        }

        public struct Details
        {
            public string subtotal;
        }

        public struct Amount
        {
            public string currency;
            public string total;
            public Details details;
        }

        public struct Transaction
        {
            public Amount amnt;
            public string description;
            public List<Item> itemList;
            public string invoiceNo;
        }

        public struct FundingInstrument
        {
            public CreditCard creditCard;
        }

        public struct Payer
        {
            public List<FundingInstrument> fundingInstrumentList;
            public string payment_method;
        }

        public struct Payment
        {
            public string intent;
            public Payer payer;
            public List<Transaction> transactions;
        }

    }
}