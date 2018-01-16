using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.ViewModels;
using System.Text;
using System.Web.Util;
using PayPal.Api;
using log4net.Repository.Hierarchy;

namespace ASPJ_Project.Controllers
{
    public class ShopController : Controller
    {
        private PayPal.Api.Payment payment;
        // GET: Shop
        public ActionResult Shop()
        {
            var shopItems = new List<PremiumShop.PremiumItem>
            {
                new PremiumShop.PremiumItem{ itemName= "Fedora Hat", itemType= "Hat", itemDescription= "Mi'lady.", beansPrice= 100},
                new PremiumShop.PremiumItem{ itemName= "Karate Headband", itemType= "Hat", itemDescription= "Hiya!", beansPrice= 75}
            };

            var currentUser = new PremiumShop.User { username = "jhn905", beansAmount = 400 };

            return View();
        }

        public void ItemPurchase()
        {
            //methods related to purchasing the item with beans
            //add item ID to user's database to display that they have the item
        }

        public ActionResult BeansPurchase()
        {
            return View();
        }

        public ActionResult PurchaseConfirmation(string username, int beansAmount, double price)
        {
            ViewData["username"] = username;
            ViewData["beansAmount"] = beansAmount;
            ViewData["price"] = price;


            return View();
        }

        public ActionResult CreditCardInfo()
        {
            return View();
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<PayPal.Api.Item>() };

            itemList.items.Add(new PayPal.Api.Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "5",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "5"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = "7", // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            return View("SuccessView");
        }

        public Boolean CheckBeans(PremiumShop.User user) //check if beans amount is more than the amount of beans the item is when they are purchasing
        {
            int userBeans;
            int itemPrice;
            Boolean success = false;
            //if (userBeans < itemPrice)
            //{
            //    success = false;
            //}
            //else if (userBeans > itemPrice)
            //{
            //    success = true;
            //}
            //else
            //    success = false;

            return success;
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
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