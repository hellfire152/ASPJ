﻿using System;
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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace ASPJ_Project.Controllers
{
    public class ShopController : Controller
    {
        private PayPal.Api.Payment payment;

        public static class CultureHelper
        {

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
            //Initialize Database Instance
            Database d = Database.CurrentInstance;

            //Initialize UserID
            int userID = Convert.ToInt32(Session["userID"].ToString());

            //Create list for storing HatItems, OutfitItems, UserItems to pass to shop via ViewBag
            List<PremiumItem> HatItems = new List<PremiumItem>();
            List<PremiumItem> OutfitItems = new List<PremiumItem>();
            List<PremiumItem> UserItems = new List<PremiumItem>();
            int userBeans = 0;
            string username = "";
            try
            {
                if (d.OpenConnection())
                {
                    string itemQuery = "SELECT * FROM premiumitem";
                    MySqlCommand c = new MySqlCommand(itemQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            //If ItemType is Hat pass it into HatItems List
                            if (r["itemType"].ToString().Equals("Hat"))
                            {
                                PremiumItem HatItem = new PremiumItem
                                {
                                    itemID = (r["itemID"].ToString()),
                                    itemName = (r["itemName"].ToString()),
                                    itemDescription = (r["itemDescription"].ToString()),
                                    beansPrice = Convert.ToInt32(r["beansPrice"])
                                };
                                HatItems.Add(HatItem);
                                ViewBag.HatItemData = HatItems;
                            }

                            //If ItemType is Outfit pass it into OutfitItems List
                            else if (r["itemType"].ToString().Equals("Outfit"))
                            {
                                PremiumItem OutfitItem = new PremiumItem
                                {
                                    itemID = (r["itemID"].ToString()),
                                    itemName = (r["itemName"].ToString()),
                                    itemDescription = (r["itemDescription"].ToString()),
                                    beansPrice = Convert.ToInt32(r["beansPrice"])
                                };

                                OutfitItems.Add(OutfitItem);
                                ViewBag.OutfitItemData = OutfitItems;
                            }
                        }
                        r.Close();
                    }

                    MySqlCommand c2 = new MySqlCommand("SELECT * FROM inventory WHERE userID = @userID", d.conn);
                    c2.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader r2 = c2.ExecuteReader())
                    {
                        while (r2.Read())
                        {
                            PremiumItem UserItem = new PremiumItem
                            {
                                itemID = (r2["itemID"].ToString()),
                            };
                            UserItems.Add(UserItem);
                        }
                        r2.Close();
                    }
                    ViewBag.UserItemsData = UserItems;

                    MySqlCommand c3 = new MySqlCommand("SELECT * FROM users where userName = @userName", d.conn);
                    c3.Parameters.AddWithValue("@userName", Session["uname"].ToString());
                    using (MySqlDataReader r3 = c3.ExecuteReader())
                    {
                        while (r3.Read())
                        {
                            userBeans = Convert.ToInt32(r3["beansAmount"]);
                            username = r3["userName"].ToString();
                        }
                        r3.Close();
                    }
                    Session["userBeans"] = userBeans;
                    Session["username"] = username;
                }
            }

            catch (MySqlException e)
            {
                Debug.WriteLine(e);
                return RedirectToAction("FailureView");
            }
            finally
            {
                d.CloseConnection();
            }

            return View();
        }

        public ActionResult Inventory()
        {
            Database d = Database.CurrentInstance;

            List<int> itemIDs = new List<int>();
            int userID = Convert.ToInt32(Session["UserID"]);
            List<PremiumItem> HatItems = new List<PremiumItem>();
            List<PremiumItem> OutfitItems = new List<PremiumItem>();
            EquippedItem equipment = new EquippedItem();
            int userBeans = 0;
            string username = "";
            try
            {
                if (d.OpenConnection())
                {
                    string inventoryQuery = "SELECT * FROM inventory where userID = @userID";
                    MySqlCommand c = new MySqlCommand(inventoryQuery, d.conn);
                    c.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            itemIDs.Add(Convert.ToInt32(r["itemID"]));
                        }
                        r.Close();
                    }

                    for (int i = 0; i < itemIDs.Count(); i++)
                    {
                        MySqlCommand c2 = new MySqlCommand("select * from premiumitem where itemID = @itemID", d.conn);
                        c2.Parameters.AddWithValue("@itemID", itemIDs[i]);
                        MySqlDataReader reader = c2.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["itemType"].ToString() == "Hat")
                            {
                                PremiumItem HatItem = new PremiumItem
                                {
                                    itemName = (reader["itemName"].ToString()),
                                    itemDescription = (reader["itemDescription"].ToString()),
                                    itemID = (reader["itemID"].ToString())
                                };
                                HatItems.Add(HatItem);
                            }

                            if (reader["itemType"].ToString() == "Outfit")
                            {
                                PremiumItem OutfitItem = new PremiumItem
                                {
                                    itemName = (reader["itemName"].ToString()),
                                    itemDescription = (reader["itemDescription"].ToString()),
                                    itemID = (reader["itemID"].ToString())
                                };
                                OutfitItems.Add(OutfitItem);
                            }
                        }
                        reader.Close();
                    }
                    ViewBag.OwnedOutfitItemData = OutfitItems;
                    ViewBag.OwnedHatItemData = HatItems;

                    MySqlCommand c3 = new MySqlCommand("SELECT * FROM equippeditems WHERE userID = @userID", d.conn);
                    c3.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader r2 = c3.ExecuteReader())
                    {

                        while (r2.Read())
                        {
                            if (r2["userID"] != DBNull.Value)
                            {
                                if (r2["equippedHat"] != DBNull.Value)
                                {
                                    equipment.equippedHat = Convert.ToInt32(r2["equippedHat"]);
                                }
                                if (r2["equippedOutfit"] != DBNull.Value)
                                {
                                    equipment.equippedOutfit = Convert.ToInt32(r2["equippedOutfit"]);
                                }
                            }
                            else break;
                        }

                        ViewBag.EquipmentData = equipment;
                        r2.Close();
                    }
                    MySqlCommand c4 = new MySqlCommand("SELECT * FROM users where userID = @userID", d.conn);
                    c4.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader r3 = c4.ExecuteReader())
                    {
                        while (r3.Read())
                        {
                            userBeans = Convert.ToInt32(r3["beansAmount"]);
                            username = r3["userName"].ToString();
                        }
                        r3.Close();
                    }
                    Session["userBeans"] = userBeans;
                    Session["username"] = username;
                }
            }

            catch (MySqlException e)
            {
                Debug.WriteLine(e);
                return RedirectToAction("FailureView");
            }
            finally
            {
                d.CloseConnection();
            }

            return View();
        }

        public ActionResult EquipItem(string equipitemID, string equipitemType)
        {

            Database d = Database.CurrentInstance;
            int userID = Convert.ToInt32(Session["UserID"]);
            int itemID = Convert.ToInt32(equipitemID);
            Debug.WriteLine(equipitemID + "equipitemID");
            Debug.WriteLine(itemID + "ITEMID");
            try
            {
                if (d.OpenConnection())
                {
                    //Query on whether EquipItemTable is existing or not
                    string tableQuery = "SELECT * FROM equippeditems WHERE userID = @userID"; 
                    MySqlCommand c = new MySqlCommand(tableQuery, d.conn);
                    c.Parameters.AddWithValue("@userID", userID);
                    bool EquipTableCreated = false;

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            EquipTableCreated = true;
                            Debug.WriteLine("Table alr created");
                        }
                        else
                        {
                            EquipTableCreated = false;
                            Debug.WriteLine("Table not created");
                        }
                        r.Close();
                    }

                    if (EquipTableCreated == true)
                    {
                        if (equipitemType == "Hat")
                        {
                            Debug.WriteLine("Hat Item Table Created");

                            string updateQuery = "UPDATE equippeditems SET equippedHat = @hatID WHERE userID = @userID";
                            MySqlCommand c2 = new MySqlCommand(updateQuery, d.conn);
                            c2.Parameters.AddWithValue("@hatID", itemID);
                            c2.Parameters.AddWithValue("@userID", userID);
                            c2.ExecuteNonQuery();
                        }

                        else if (equipitemType == "Outfit")
                        {
                            Debug.WriteLine("Outfit Item Table Created");

                            string updateQuery = "UPDATE equippeditems SET equippedOutfit = @outfitID WHERE userID = @userID";
                            MySqlCommand c2 = new MySqlCommand(updateQuery, d.conn);
                            c2.Parameters.AddWithValue("@outfitID", itemID);
                            c2.Parameters.AddWithValue("@userID", userID);
                            c2.ExecuteNonQuery();
                        }
                    }

                    else if (EquipTableCreated == false)
                    {
                        if (equipitemType == "Hat")
                        {
                            Debug.WriteLine("Hat Item Table not Created");

                            string insertQuery = "INSERT INTO equippeditems(userID, equippedHat) VALUES (@userID, @hatID)";
                            MySqlCommand c2 = new MySqlCommand(insertQuery, d.conn);
                            c2.Parameters.AddWithValue("@hatID", itemID);
                            c2.Parameters.AddWithValue("@userID", userID);
                            c2.ExecuteNonQuery();
                        }

                        else if (equipitemType == "Outfit")
                        {
                            Debug.WriteLine("Outfit Item Table not Created");
                            string insertQuery = "INSERT INTO equippeditems(userID, equippedOutfit) VALUES (@userID, @outfitID)";
                            MySqlCommand c2 = new MySqlCommand(insertQuery, d.conn);
                            c2.Parameters.AddWithValue("@outfitID", itemID);
                            c2.Parameters.AddWithValue("@userID", userID);
                            c2.ExecuteNonQuery();
                        }
                    }
                }
            }

            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                d.CloseConnection();
            }

            return RedirectToAction("Inventory");

        }

        public ActionResult ItemPurchase(string beansPrice, string premiumItemName, string premiumItemID)
        {

            Database d = Database.CurrentInstance;
            int userID = Convert.ToInt32(Session["UserID"]);
            int itemID = Convert.ToInt32(premiumItemID);

            try
            {
                if (d.OpenConnection())
                {
                    string userQuery = "SELECT * FROM users WHERE userID = @userID";
                    MySqlCommand c = new MySqlCommand(userQuery, d.conn);
                    c.Parameters.AddWithValue("@userID", userID);
                    int beansBefore = 0;
                    int beansAfter = 0;
                    bool successfulPurchase = false;

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (Convert.ToInt32(r["userID"]) == userID)
                            {
                                beansBefore = Convert.ToInt32(r["beansAmount"].ToString());
                                beansAfter = beansBefore - Convert.ToInt32(beansPrice);
                                if (beansAfter < 0)
                                {
                                    successfulPurchase = false;
                                }

                                else if (beansAfter > 0)
                                {
                                    successfulPurchase = true;
                                }
                            }
                        }
                        r.Close();

                        if (successfulPurchase == true)
                        {
                            AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
                            string updateQuery = "UPDATE users SET beansAmount = @beansAfter WHERE userID = @userID";
                            MySqlCommand c2 = new MySqlCommand(updateQuery, d.conn);
                            c2.Parameters.AddWithValue("@beansAfter", beansAfter);
                            c2.Parameters.AddWithValue("@userID", userID);
                            c2.ExecuteNonQuery();

                            string addItemTransQuery = "INSERT INTO itemtransaction VALUES (@transactionNo, @transactionDesc, @price, @beansBefore, @beansAfter, @userID, @dateOfTransaction)";
                            string transDesc = "Purchase of " + premiumItemName + " for " + beansPrice + " beans.";
                            MySqlCommand c3 = new MySqlCommand(addItemTransQuery, d.conn);
                            c3.Parameters.AddWithValue("@transactionNo", AES.AesEncrypt(KeyGenerator.GetUniqueKey(20)));
                            c3.Parameters.AddWithValue("@transactionDesc", AES.AesEncrypt(transDesc));
                            c3.Parameters.AddWithValue("@price", beansPrice);
                            c3.Parameters.AddWithValue("@beansBefore", beansBefore);
                            c3.Parameters.AddWithValue("@beansAfter", beansAfter);
                            c3.Parameters.AddWithValue("@userID", AES.AesEncrypt(userID.ToString()));
                            c3.Parameters.AddWithValue("@dateOfTransaction", DateTime.Now);
                        
                            c3.ExecuteNonQuery();

                            string addInventoryQuery = "INSERT INTO inventory VALUES (@userID, @itemID)";
                            MySqlCommand c4 = new MySqlCommand(addInventoryQuery, d.conn);
                            c4.Parameters.AddWithValue("@userID", userID);
                            c4.Parameters.AddWithValue("@itemID", itemID);
                            c4.ExecuteNonQuery();
                        }

                        else
                        {
                            return RedirectToAction("FailureView");
                        }
                    }
                }
            }

            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                d.CloseConnection();
            }

            return RedirectToAction("Shop");

        }

        public ActionResult BeansPurchase()
        {
            Database d = Database.CurrentInstance;
            int userID = Convert.ToInt32(Session["UserID"]);
            List<PremiumItem> beanPurchases = new List<PremiumItem>();
            int userBeans = 0;
            string username = "";
            try
            {
                if (d.OpenConnection())
                {
                    string purchasesQuery = "SELECT * FROM beanpurchases";
                    MySqlCommand c = new MySqlCommand(purchasesQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            PremiumItem purchase = new PremiumItem
                            {
                                itemName = (r["beanName"].ToString()),
                                priceOfBeans = Convert.ToDouble(r["beanPrice"]),
                                beanAmount = Convert.ToInt32(r["beanAmount"]),
                                beanIcon = (r["beanIcon"].ToString())
                            };
                            beanPurchases.Add(purchase);
                            Debug.WriteLine(purchase.priceOfBeans);
                        }
                        r.Close();
                        ViewBag.BeanPurchasesData = beanPurchases;
                    }

                    MySqlCommand c4 = new MySqlCommand("SELECT * FROM users where userID = @userID", d.conn);
                    c4.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader r3 = c4.ExecuteReader())
                    {
                        while (r3.Read())
                        {
                            userBeans = Convert.ToInt32(r3["beansAmount"]);
                            username = r3["userName"].ToString();
                        }
                        r3.Close();
                    }
                    Session["userBeans"] = userBeans;
                    Session["username"] = username;
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                d.CloseConnection();
            }

            return View();
        }

        public ActionResult PurchaseSession(int beansAmount, double price, string beansName)
        {
            Session["beansName"] = beansName;
            Session["beansAmount"] = beansAmount;
            Session["price"] = price;

            return RedirectToAction("PurchaseConfirmation");
        }

        public ActionResult PurchaseConfirmation()
        {
            Database d = Database.CurrentInstance;
            int userBeans = 0;
            string username = "";
            int userID = Convert.ToInt32(Session["UserID"]);

            try
            {
                if (d.OpenConnection())
                {

                    MySqlCommand c4 = new MySqlCommand("SELECT * FROM users where userID = @userID", d.conn);
                    c4.Parameters.AddWithValue("@userID", userID);
                    using (MySqlDataReader r3 = c4.ExecuteReader())
                    {
                        while (r3.Read())
                        {
                            userBeans = Convert.ToInt32(r3["beansAmount"]);
                            username = r3["userName"].ToString();
                        }
                        r3.Close();
                    }
                    Session["userBeans"] = userBeans;
                    Session["username"] = username;
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                d.CloseConnection();
            }

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

            Session["ShopSessionID1"] = KeyGenerator.GetUniqueKey(20);

            string sessionID1 = Session["ShopSessionID1"].ToString();

            Session["ShopSessionID2"] = BCrypt.HashSession(sessionID1, BCrypt.GenerateSalt());

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
                item.name = beansName + " (" + beansAmount + ") Beans";
                item.currency = "SGD";
                item.price = price;
                item.quantity = "1";
                item.sku = KeyGenerator.GetUniqueKey(20);

                //Now make a List of Item and add the above item to it
                //you can create as many items as you want and add to this list
                List<PayPal.Api.Item> itms = new List<PayPal.Api.Item>();
                itms.Add(item);
                ItemList itemList = new ItemList();
                itemList.items = itms;

                //Address for the payment
                Address billingAddress = new Address
                {
                    city = currentCard.billing_address.city,
                    country_code = "SG",
                    line1 = currentCard.billing_address.line1,
                    line2 = currentCard.billing_address.line2,
                    postal_code = currentCard.billing_address.postal_code,
                    state = currentCard.billing_address.state
                };


                //Now Create an object of credit card and add above details to it
                //Please replace your credit card details over here which you got from paypal
                PayPal.Api.CreditCard crdtCard = new PayPal.Api.CreditCard
                {
                    billing_address = billingAddress,
                    cvv2 = currentCard.cvv2,  //card cvv2 number
                    expire_month = currentCard.expire_month, //card expire date
                    expire_year = currentCard.expire_year, //card expire year
                    first_name = currentCard.first_name,
                    last_name = currentCard.last_name,
                    number = currentCard.creditCardNo //enter your credit card number here
                };
                if (Regex.IsMatch(currentCard.creditCardNo, "^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$"))
                {
                    crdtCard.type = "mastercard";
                }

                if (Regex.IsMatch(currentCard.creditCardNo, "^4[0-9]{12}(?:[0-9]{3})?$"))
                {
                    crdtCard.type = "visa";
                }

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
                tran.description = "Purchase of " + beansAmount + " beans. Beans will be added after successful purchase.";
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
                Payer payr = new Payer
                {
                    funding_instruments = fundingInstrumentList,
                    payment_method = "credit_card"
                };

                // finally create the payment object and assign the payer object & transaction list to it
                Payment pymnt = new Payment
                {
                    intent = "sale",
                    payer = payr,
                    transactions = transactions
                };

                try
                {
                    //getting context from the paypal
                    //basically we are sending the clientID and clientSecret key in this function
                    //to the get the context from the paypal API to make the payment
                    //for which we have created the object above.

                    //Basically, apiContext object has a accesstoken which is sent by the paypal
                    //to authenticate the payment to facilitator account.
                    //An access token could be an alphanumeric string

                    APIContext apiContext = Models.Configuration.GetAPIContext();

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
                    Debug.WriteLine(ex);
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
                name = beansName + " (" + beansAmount + " Beans)",
                currency = "SGD",
                price = price,
                quantity = "1",
                sku = KeyGenerator.GetUniqueKey(20)
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
                description = "Purchase of " + beansAmount + " beans. Beans will be added after successful purchase.",
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

        [ValidateInput(false)]
        public ActionResult PaymentWithPaypal(Models.CreditCard currentCard)
        {
            Database d = Database.CurrentInstance;
            AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
            int userID = Convert.ToInt32(Session["UserID"]);
            string price = string.Empty;
            price = Convert.ToString(Session["price"]);
            string beansName = string.Empty;
            beansName = Convert.ToString(Session["beansName"]);
            string beansAmount = string.Empty;
            beansAmount = Convert.ToString(Session["beansAmount"]);

            //getting the apiContext as earlier
            APIContext apiContext = Models.Configuration.GetAPIContext();

            //generating sessionID
            Session["ShopSessionID1"] = KeyGenerator.GetUniqueKey(20);

            string sessionID1 = Session["ShopSessionID1"].ToString();

            Session["ShopSessionID2"] = BCrypt.HashSession(sessionID1, BCrypt.GenerateSalt());

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
                        string addItemTransQuery = "INSERT INTO beantransaction(transactionNo, transactionDesc, priceOfBeans, status, dateOfTransaction, userID) VALUES (@transactionNo, @transactionDesc, @price, @status, @dateOfTransaction, @userID)";
                        string transDesc = "Failed Purchase of " + beansName + " (" + beansAmount + " Beans) for $" + price;
                        MySqlCommand c3 = new MySqlCommand(addItemTransQuery, d.conn);

                        c3.Parameters.AddWithValue("@transactionNo", AES.AesEncrypt(KeyGenerator.GetUniqueKey(20)));
                        c3.Parameters.AddWithValue("@transactionDesc", AES.AesEncrypt(transDesc));
                        c3.Parameters.AddWithValue("@price", Convert.ToDouble(price));
                        c3.Parameters.AddWithValue("@status", "Failure");
                        c3.Parameters.AddWithValue("@dateOfTransaction", DateTime.Now);
                        c3.Parameters.AddWithValue("@userID", AES.AesEncrypt(userID.ToString()));
                        return RedirectToAction("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                string addItemTransQuery = "INSERT INTO beantransaction(transactionNo, transactionDesc, priceOfBeans, status, dateOfTransaction, userID) VALUES (@transactionNo, @transactionDesc, @price, @status, @dateOfTransaction, @userID)";
                string transDesc = "Failed Purchase of " + beansName + " (" + beansAmount + " Beans) for $" + price;
                MySqlCommand c3 = new MySqlCommand(addItemTransQuery, d.conn);

                c3.Parameters.AddWithValue("@transactionNo", AES.AesEncrypt(KeyGenerator.GetUniqueKey(20)));
                c3.Parameters.AddWithValue("@transactionDesc", AES.AesEncrypt(transDesc));
                c3.Parameters.AddWithValue("@price", Convert.ToDouble(price));
                c3.Parameters.AddWithValue("@status", "Failure");
                c3.Parameters.AddWithValue("@dateOfTransaction", DateTime.Now);
                c3.Parameters.AddWithValue("@userID", AES.AesEncrypt(userID.ToString()));
                return View("FailureView");
            }

            try
            {
                if (d.OpenConnection())
                {
                    string userQuery = "SELECT * FROM users WHERE userID = @userID";
                    MySqlCommand c = new MySqlCommand(userQuery, d.conn);
                    c.Parameters.AddWithValue("@userID", userID);
                    int beansBefore = 0;
                    int beansAfter = 0;

                    Debug.WriteLine("SCARY");
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (Convert.ToInt32(r["userID"]) == userID)
                            {
                                beansBefore = Convert.ToInt32(r["beansAmount"].ToString());
                                beansAfter = beansBefore + Convert.ToInt32(beansAmount);
                            }
                        }
                        r.Close();

                        string updateQuery = "UPDATE users SET beansAmount = @beansAfter WHERE userID = @userID";
                        MySqlCommand c2 = new MySqlCommand(updateQuery, d.conn);

                        c2.Parameters.AddWithValue("@beansAfter", beansAfter);
                        c2.Parameters.AddWithValue("@userID", userID);
                        c2.ExecuteNonQuery();
                        Debug.WriteLine(beansBefore + " " + beansAfter);

                        string addItemTransQuery = "INSERT INTO beantransaction VALUES (@transactionNo, @transactionDesc, @price, @beansBefore, @beansAfter, @status, @dateOfTransaction, @userID)";
                        string transDesc = "Purchase " + beansName + " (" + beansAmount + " Beans) for $" + price;
                        MySqlCommand c3 = new MySqlCommand(addItemTransQuery, d.conn);

                        c3.Parameters.AddWithValue("@transactionNo", AES.AesEncrypt(KeyGenerator.GetUniqueKey(20)));
                        c3.Parameters.AddWithValue("@transactionDesc", AES.AesEncrypt(transDesc));
                        c3.Parameters.AddWithValue("@price", Convert.ToDouble(price));
                        c3.Parameters.AddWithValue("@beansBefore", beansBefore);
                        c3.Parameters.AddWithValue("@beansAfter", beansAfter);
                        c3.Parameters.AddWithValue("@status", "Successful");
                        c3.Parameters.AddWithValue("@dateOfTransaction", DateTime.Now);
                        c3.Parameters.AddWithValue("@userID", AES.AesEncrypt(userID.ToString()));

                        c3.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e);

                string addItemTransQuery = "INSERT INTO beantransaction(transactionNo, transactionDesc, priceOfBeans, status, dateOfTransaction, userID) VALUES (@transactionNo, @transactionDesc, @price, @status, @dateOfTransaction, @userID)";
                string transDesc = "Failed Purchase of " + beansName + " (" + beansAmount + " Beans) for $" + price;
                MySqlCommand c3 = new MySqlCommand(addItemTransQuery, d.conn);

                c3.Parameters.AddWithValue("@transactionNo", AES.AesEncrypt(KeyGenerator.GetUniqueKey(20)));
                c3.Parameters.AddWithValue("@transactionDesc", AES.AesEncrypt(transDesc));
                c3.Parameters.AddWithValue("@price", Convert.ToDouble(price));
                c3.Parameters.AddWithValue("@status", "Failure");
                c3.Parameters.AddWithValue("@dateOfTransaction", DateTime.Now);
                c3.Parameters.AddWithValue("@userID", AES.AesEncrypt(userID.ToString()));

                return RedirectToAction("FailureView");
            }
            finally
            {
                d.CloseConnection();
            }
            return RedirectToAction("SuccessView");
        }

        public ActionResult SuccessView()
        {
            if (Session["ShopSessionID1"] == null || Session["ShopSessionID2"] == null)
            {
                return RedirectToAction("FailureView");
            }
            else
            {
                string sessionID1 = Session["ShopSessionID1"].ToString();
                string sessionID2 = Session["ShopSessionID2"].ToString();

                bool doSessionsMatch = BCrypt.CheckSession(sessionID1, sessionID2);
                if (doSessionsMatch == true)
                {
                    Session["ShopSessionID1"] = null;
                    Session["ShopSessionID2"] = null;
                    return View();
                }
                else
                {
                    return RedirectToAction("FailureView");
                }
            }
        }

        public ActionResult FailureView()
        {
            Session["ShopSessionID1"] = null;
            Session["ShopSessionID2"] = null;
            return View();
        }

        public Boolean CheckBeans(PremiumShop.User user, PremiumItem premiumItem) //check if beans amount is more than the amount of beans the item is when they are purchasing
        {
            double userBeans = user.beansAmount;
            double itemPrice = premiumItem.beansPrice;
            Boolean success = false;

            if (userBeans < itemPrice)
            {
                success = false;
            }
            else if (userBeans > itemPrice)
            {
                success = true;
            }
            else
                success = false;

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