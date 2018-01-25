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
using System.Globalization;
using System.Diagnostics;

namespace ASPJ_Project.Controllers
{
    public class ShopController : Controller
    {
        private PayPal.Api.Payment payment;
        // GET: Shop

    public static class CultureHelper {

            public static Dictionary<string, string> CountryList()
            {
                //Creating Dictionary
                Dictionary<string, string> cultureList = new Dictionary<string, string>();

                //getting the specific CultureInfo from CultureInfo class
                CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                foreach (CultureInfo getCulture in getCultureInfo)
                {
                    //creating the object of RegionInfo class
                    RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                    //adding each country Name into the Dictionary
                    if (!(cultureList.ContainsKey(getRegionInfo.Name)))
                    {
                        cultureList.Add(getRegionInfo.Name, getRegionInfo.EnglishName);
                    }
                }
                //returning country list
                return cultureList;
            }
        }
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

        public ActionResult PurchaseConfirmation(string username, int beansAmount, double price, string beansName)
        {
            ViewData["username"] = username;
            ViewData["beansName"] = beansName;
            ViewData["beansAmount"] = beansAmount;
            ViewData["price"] = price;

            PremiumShop.BeanAndPrice beansPrice = new PremiumShop.BeanAndPrice
            {
                beans = beansAmount,
                price = price,
                beansName = beansName
            };

            Session["username"] = username;
            Session["beansName"] = beansName;
            Session["beansAmount"] = beansAmount;
            Session["price"] = price;
            return View();
        }
        public ActionResult CreditCardInfo()
        {
            var list = new SelectList(CultureHelper.CountryList(), "Key", "Value");
            var sortList = list.OrderBy(p => p.Text).ToList();
            ViewBag.Countries = sortList;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreditCardInfo(Models.CreditCard currentCard)
        {
            if (string.IsNullOrEmpty(currentCard.creditCardNo))
            {
                ModelState.AddModelError("creditCardNo", "Credit card number is required.");
            }
            if (string.IsNullOrEmpty(currentCard.cvv2))
            {
                ModelState.AddModelError("creditCardNo", "CVV is required.");
            }
            if (string.IsNullOrEmpty(currentCard.first_name))
            {
                ModelState.AddModelError("creditCardNo", "First Name is required.");
            }
            if (string.IsNullOrEmpty(currentCard.last_name))
            {
                ModelState.AddModelError("creditCardNo", "Last Name is required.");
            }
            if (ModelState.IsValid)
            {
                //create and item for which you are taking payment
                //if you need to add more items in the list
                //Then you will need to create multiple item objects or use some loop to instantiate object

                string price = string.Empty;
                price = Convert.ToString(Session["price"]);

                string beansName = string.Empty;
                price = Convert.ToString(Session["beansName"]);

                string beansAmount = string.Empty;
                price = Convert.ToString(Session["beansAmount"]);

                PayPal.Api.Item item = new PayPal.Api.Item();
                item.name = beansName + " (" + beansAmount + ")";
                item.currency = "SGD";
                item.price = price;
                item.quantity = "1";
                item.sku = "sku";

                //Now make a List of Item and add the above item to it
                //you can create as many items as you want and add to this list
                List<PayPal.Api.Item> itms = new List<PayPal.Api.Item>();
                itms.Add(item);
                ItemList itemList = new ItemList();
                itemList.items = itms;

                //Address for the payment
                Address billingAddress = new Address();
                billingAddress.city = currentCard.billing_address.city;
                billingAddress.country_code = currentCard.billing_address.country_code;
                billingAddress.line1 = currentCard.billing_address.line1;
                billingAddress.line2 = currentCard.billing_address.line2;
                billingAddress.postal_code = currentCard.billing_address.postal_code;
                billingAddress.state = currentCard.billing_address.state;


                //Now Create an object of credit card and add above details to it
                //Please replace your credit card details over here which you got from paypal
                PayPal.Api.CreditCard crdtCard = new PayPal.Api.CreditCard();
                crdtCard.billing_address = billingAddress;
                crdtCard.cvv2 = currentCard.cvv2;  //card cvv2 number
                crdtCard.expire_month = currentCard.expire_month; //card expire date
                crdtCard.expire_year = currentCard.expire_year; //card expire year
                crdtCard.first_name = currentCard.first_name;
                crdtCard.last_name = currentCard.last_name;
                crdtCard.number = currentCard.creditCardNo; //enter your credit card number here
                crdtCard.type = currentCard.type; //credit card type here paypal allows 4 types

                // Specify details of your payment amount.
                Details details = new Details();
                details.shipping = "0";
                details.subtotal = price;
                details.tax = "0";

                // Specify your total payment amount and assign the details object
                Amount amnt = new Amount();
                amnt.currency = "SGD";
                // Total = shipping tax + subtotal.
                amnt.total = price;
                amnt.details = details;

                // Now make a transaction object and assign the Amount object
                Transaction tran = new Transaction();


                tran.amount = amnt;
                tran.description = "Description about the payment amount.";
                tran.item_list = itemList;
                tran.invoice_number = KeyGenerator.GetUniqueKey(20);

                // Now, we have to make a list of transaction and add the transactions object
                // to this list. You can create one or more object as per your requirements

                List<Transaction> transactions = new List<Transaction>();
                transactions.Add(tran);

                // Now we need to specify the FundingInstrument of the Payer
                // for credit card payments, set the CreditCard which we made above

                FundingInstrument fundInstrument = new FundingInstrument();
                fundInstrument.credit_card = crdtCard;

                // The Payment creation API requires a list of FundingIntrument

                List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
                fundingInstrumentList.Add(fundInstrument);

                // Now create Payer object and assign the fundinginstrument list to the object
                Payer payr = new Payer();
                payr.funding_instruments = fundingInstrumentList;
                payr.payment_method = "credit_card";

                // finally create the payment object and assign the payer object & transaction list to it
                Payment pymnt = new Payment();
                pymnt.intent = "sale";
                pymnt.payer = payr;
                pymnt.transactions = transactions;

                try
                {
                    //getting context from the paypal
                    //basically we are sending the clientID and clientSecret key in this function
                    //to the get the context from the paypal API to make the payment
                    //for which we have created the object above.

                    //Basically, apiContext object has a accesstoken which is sent by the paypal
                    //to authenticate the payment to facilitator account.
                    //An access token could be an alphanumeric string

                    APIContext apiContext = Configuration.GetAPIContext();

                    //Create is a Payment class function which actually sends the payment details
                    //to the paypal API for the payment. The function is passed with the ApiContext
                    //which we received above.

                    Payment createdPayment = pymnt.Create(apiContext);

                    //if the createdPayment.state is "approved" it means the payment was successful 

                    if (createdPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
                catch (PayPal.PayPalException ex)
                {
                    return View("FailureView");
                }

                return View("SuccessView");
            }
            else
            {
                return View(currentCard);
            }
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<PayPal.Api.Item>() };
            string price = string.Empty;
            price = Convert.ToString(Session["price"]);

            string beansName = string.Empty;
            beansName = Convert.ToString(Session["beansName"]);

            string beansAmount = string.Empty;
            beansAmount = Convert.ToString(Session["beansAmount"]);
            itemList.items.Add(new PayPal.Api.Item()
            {
                name = beansName + " (" + beansAmount + ")",
                currency = "SGD",
                price =  price,
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
                tax = "0",
                shipping = "0",
                subtotal = price
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "SGD",
                total = price, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = KeyGenerator.GetUniqueKey(20),
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


        public ActionResult PaymentWithPaypal(Models.CreditCard currentCard)
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
                                "/Shop/PaymentWithPayPal?";

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

    }
}