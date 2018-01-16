using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using PayPal.Api;

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
            public string creditCardNo;
        }

        public struct Address
        {
            public string city;
            public string country_code;
            public string line1;
            public string line2;
            public string postal_code;
            public string state;
        }

        public struct CreditCard
        {
            public Address billing_address;
            public string cvv2;
            public int expire_month;
            public int expire_year;
            public string first_name;
            public string last_name;
            public string creditCardNo;
            public string type;
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