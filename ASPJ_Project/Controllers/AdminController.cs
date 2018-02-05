using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using MySql.Data.MySqlClient;

namespace ASPJ_Project.Controllers
{
    public class AdminController : Controller
    {
        
        // GET: Admin
        public ActionResult Index()
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion

            return View();
        }

        
        
        public ActionResult TransactionHistory()
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion

            Database d = Database.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    List<TransactionHistory> transactions = new List<TransactionHistory>();
                    string SearchQuery = "SELECT * FROM dububase.beantransaction Order by dateOfTransaction Desc";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            transactions.Add(new TransactionHistory
                            {
                                TransactionNo =AES.AesDecrypt(r["transactionNo"].ToString()),
                                TransactionDesc = AES.AesDecrypt(r["transactionDesc"].ToString()),
                                Price = Convert.ToDouble(r["priceOfItem"]),
                                Status = r["status"].ToString(),
                                BeansBefore = Convert.ToInt32(r["userBeansBefore"]),
                                BeansAfter = Convert.ToInt32(r["userBeansAfter  "]),
                                DateOfTransaction = (Convert.ToDateTime(r["dateOfTransaction"])).ToString(),
                                UserID = AES.AesDecrypt(r["userId"].ToString())
                            });
                        }
                    }
                    ViewBag.Transactions = transactions;
                    return View();
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }

            return View();
        }

        public ActionResult Search()
        {
            #region THE ROLE BASE CODE

            // remove if admin is not needed for the page
            //Session["role"] = "Admin";
            //string role = Session["role"].ToString();
            //if (role != "Admin")
            //{
            //    return RedirectToAction("Index", "Unauthorized");
            //}

            //if user is not logged in
            //if (Session["uname"].ToString() == "" || Session["uname"].ToString() == null)
            //{
            //    return RedirectToAction("login", "user");
            //}

            //if user is ban
            //Session["isBan"] = "True";
            //string isBan = Session["isBan"].ToString();
            //if (isBan == "true")
            //{
            //    Database f = Database.CurrentInstance;
            //    try
            //    {
            //        if (f.OpenConnection())
            //        {
            //            string SearchQuery = "SELECT * FROM dububase.users where username=@username";

            //            MySqlCommand c = new MySqlCommand(SearchQuery, f.conn);
            //            c.Parameters.AddWithValue("@username", Session["username"]);
            //            using (MySqlDataReader r = c.ExecuteReader())
            //            {
            //                while (r.Read())
            //                {
            //                    var date = DateTime.Parse(r["banTill"].ToString());
            //                    if (DateTime.Compare(DateTime.Now, date) > 0)
            //                    {
            //                        SearchQuery = "UPDATE dububase.users set banTill = null,isBan = false where user = @session";
            //                        MySqlCommand cmd = new MySqlCommand(SearchQuery, f.conn);
            //                        cmd.Parameters.AddWithValue("@session", Session["username"]);
            //                        cmd.ExecuteNonQuery();
            //                    }
            //                };
            //            }
            //        }
            //    }
            //    catch (MySqlException e)
            //    {
            //        Debug.WriteLine("MySQL Error!");
            //    }
            //    finally
            //    {
            //        f.CloseConnection();
            //    };

            //    //get the banTill Period and compare if it is over than set isBan to "false"
            //    //TempData["username"] = get from eunice page database side lololol
            //    // do this at the banned details page
            //    //TempData.Clear()
            //    //redirect to ban page with reason maybe?
            //    TempData["BanTill"] = "MyMessage";//get from db the banTill;
            //    //use at the other page
            //    //ViewBag.Message = TempData["shortMessage"].ToString();
            //    return RedirectToAction("", "");
            //}
            #endregion
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            Database d = Database.CurrentInstance;
            List<DummyProfile> Dummys = new List<DummyProfile>();

            try
            {
              if (d.OpenConnection())
              {
                    string SearchQuery = "SELECT * FROM dububase.users";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    List<user> users = new List<user>();
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            user user = new user
                            {
                                userID = (Convert.ToInt32(r["UserId"])),
                                userName = (r["userName"].ToString()),
                                email = (r["email"]).ToString(),
                                firstName = (r["firstName"].ToString()),
                                lastName = (r["lastName"].ToString())
                            };
                            users.Add(user);
                            ViewBag.Dummys = users;
                        }
                    }
              }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchModel model)
        {
            #region useless
            //if (search != null)
            //{
            //    List<Player> searchlist = new List<Player>();
            //    foreach(Player i in DB){
            //        if(i.username == search)
            //        {
            //            searchlist.Add(i);
            //        }
            //    }

            //    ViewData["match"] = searchlist;
            //}


            //code from video
            //return View(db.Users.Where(x => x.Name.StartsWith(search) || search == null).toList());
            #endregion
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            #region db
            Database d = Database.CurrentInstance;

            List<DummyProfile> Dummys = new List<DummyProfile>();


            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dummyuser WHERE username LIKE %@username;";
                    //d.PNQ("SELECT * FROM dububase.users WHERE @searchtype LIKE @search%",model.SearchType, model.Search);
                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    c.Parameters.AddWithValue("@username",model.Search);
                    List<user> users = new List<user>();
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            user user = new user
                            {
                                userID = (Convert.ToInt32(r["UserId"])),
                                userName = (r["userName"].ToString()),
                                email = (r["email"]).ToString(),
                                firstName = (r["firstName"].ToString()),
                                lastName = (r["lastName"].ToString())
                            };
                            users.Add(user);
                            ViewBag.Dummys = users;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
            #endregion

        }


        public ActionResult BanUser(string username)
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            Database d = Database.CurrentInstance;
            List<DummyProfile> Dummys = new List<DummyProfile>();
            
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.users where username=@username";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    c.Parameters.AddWithValue("@username",username);
                    List<user> users = new List<user>();
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        user user = new user
                        {
                            userID = (Convert.ToInt32(r["UserId"])),
                            userName = (r["userName"].ToString()),
                            email = (r["email"]).ToString(),
                            firstName = (r["firstName"].ToString()),
                            lastName = (r["lastName"].ToString())
                        };
                        users.Add(user);
                        ViewBag.Dummys = users;
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BanUser(BanUserModel model)
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            string username = model.Username;
            //db stuff
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            AESCryptoStuff aes_obj = AESCryptoStuff.CurrentInstance;
            //EncodeDecode encInit = new EncodeDecode();
            Debug.WriteLine("this is model.banPeriod"+model.BanPeriod);
            try
            {
                string queryString = "";
                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                //chatMessageInsert.ChatMessage = encInit.EncodeStuff(chatMessageInsert.ChatMessage);
                //chatMessageInsert.ChatMessage = aes_obj.AesEncrypt(chatMessageInsert.ChatMessage);
                // queryString = "INSERT INTO dububase.chat(chatMessage) VALUES(@sendmessage)";

                #region useless
                //queryString = "Select * from dububase.users where username = @username";
                //cmd.CommandText = queryString;
                ////cmd.Parameters.AddWithValue("@sendmessage", chatMessageInsert.ChatMessage);
                //cmd.Parameters.AddWithValue("@username", username);
                //cmd.ExecuteNonQuery();
                #endregion

                //ban period calculate date
                queryString = "UPDATE dububase.users SET isBan = 'true', banTill=@date Where username=@username;";
                cmd.CommandText = queryString;
                String BanPeriod = model.BanPeriod;
                int time = 0;
                if (BanPeriod == "1 Week")
                {
                    time = 7;
                }
                else if (BanPeriod == "2 Weeks")
                {
                    time = 14;
                }
                else if (BanPeriod == "1 Month")
                {
                    time = 30;
                }
                else if (BanPeriod == "3 Months")
                {
                    time = 90;
                }
                else if (BanPeriod == "1 Year")
                {
                    time = 365;
                }
                DateTime mehgofu = DateTime.Now.AddDays(time);
                cmd.Parameters.AddWithValue("@date", mehgofu);
                cmd.Parameters.AddWithValue("@username", model.Username);
                cmd.ExecuteNonQuery();

                //add ban table into sql
                queryString = "INSERT INTO dububase.banhistory(username, banReason,banPeriod) VALUES(@username, @banReason,@banPeriod); ";
                cmd.CommandText = queryString;
                cmd.Parameters.AddWithValue("@username", model.Username);
                cmd.Parameters.AddWithValue("@banReason", model.BanReason);
                cmd.Parameters.AddWithValue("@banPeriod", model.BanPeriod);
                cmd.ExecuteNonQuery();

                return View();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                conn.Close();
            }
        }

        public ActionResult BanHistory()
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            Database d = Database.CurrentInstance;
            List<BanUserModel> Dummys = new List<BanUserModel>();

            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.banhistory";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            BanUserModel dummy = new BanUserModel
                            {
                                BanPeriod = ((r["BanPeriod"].ToString())),
                                BanReason = (r["BanReason"].ToString()),
                                Username = (r["Username"]).ToString()
                            };
                            Dummys.Add(dummy);
                            ViewBag.Dummys = Dummys;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
        }
        

        public ActionResult BanHistory(BanSearchModel model)
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            Database d = Database.CurrentInstance;
            //MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            //AESCryptoStuff aes_obj = new AESCryptoStuff();
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.banHistroy;";
                    //d.PNQ("SELECT * FROM dububase.users WHERE @searchtype LIKE @search%", model.username);
                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    List<BanUserModel> noobs = new List<BanUserModel>();
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            BanUserModel noob = new BanUserModel {
                                Username = (r["UserName"].ToString()),
                                BanReason = (r["banReason"].ToString()),
                                BanPeriod = (r["banPeriod"].ToString())
                            };
                            noobs.Add(noob);
                            ViewBag.noobs = noobs;
                        }
                    }
                }
                #region old
                //string queryString = "";
                //String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
                //conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                //conn.Open();
                //MySqlCommand cmd = new MySqlCommand(queryString, conn);

                //aes_obj.AesInitialize();
                //queryString = "Select * from dububase.bantable where username Like %@username";
                //cmd.CommandText = queryString;
                //cmd.Parameters.AddWithValue("@username", model.Username);
                //cmd.ExecuteNonQuery();
                #endregion

            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }

            return View();
        }

        [HttpGet]
        public ActionResult RoleChange(string username)
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion
            ViewBag.user = username;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleChange(ChangeRoleModel model)
        {
            #region role and is logged in
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Unauthorized");
            }


            #endregion

            Database d = Database.CurrentInstance;
            
            AESCryptoStuff aes_obj = AESCryptoStuff.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    string queryString = "UPDATE dububase.users SET role = @role Where username=@username;";
                    MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                    cmd.CommandText = queryString;
                    cmd.Parameters.AddWithValue("@role", model.NewRole);
                    cmd.Parameters.AddWithValue("@username", model.Username);
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("UserProfile","Admin",new { username = model.Username });


                }
            }catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                d.CloseConnection();
            }

            //change to somewhere
            return View();
        }

        //public ActionResult ItemAdd()
        //{
        //    #region role and is logged in
        //    if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    if (Session["role"].ToString() != "Admin")
        //    {
        //        return RedirectToAction("Index", "Unauthorized");
        //    }


        //    #endregion
        //    return View();
        //}

        //public ActionResult ItemAdd(PremiumItem item)
        //{
        //    #region role and is logged in
        //    if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    if (Session["role"].ToString() != "Admin")
        //    {
        //        return RedirectToAction("Index", "Unauthorized");
        //    }


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string SearchQuery = "Select count(itemName) As itemExist From dububase.premiumitem where itemName = @item";
        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
        //            c.Parameters.AddWithValue("@item", item.itemName);
        //            int itemExist = 0;
        //            using (MySqlDataReader r = c.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    itemExist = Convert.ToInt32(r["itemExist"].ToString());
        //                }
        //            }

        //            if (itemExist != 0)
        //            {
        //                SearchQuery = "INSERT INTO dububase.premiumitem(itemName,itemType,itemImage,itemDescription,beansPrice) VALUES(@name,@itemType,@itemImage,@itemDescription,@beansPrice);";
        //                c = new MySqlCommand(SearchQuery, d.conn);
        //                c.Parameters.AddWithValue("@itemName", item.itemName);
        //                c.Parameters.AddWithValue("@itemType", item.itemType);
        //                c.Parameters.AddWithValue("@itemDescription", item.itemDescription);
        //                c.Parameters.AddWithValue("@beansPrice", item.beansPrice);
        //                c.Parameters.AddWithValue("@itemImage", null);
        //                //c.Parameters.AddWithValue("@dateStart", item.dateStart);
        //                //c.Parameters.AddWithValue("@dateEnd", item.dateEnd);
        //                //ALEX
        //                //c.Parameters.AddWithValue("@itemImage", item.itemIamge);
        //                c.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                ViewBag.Message = "Item Name Already Exists";
        //                return View();
        //            }
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }

        //    return View("ItemManager");
        //}

        //[HttpGet]
        //public ActionResult ItemDelete(string name)
        //{
        //    #region role and is logged in
        //    if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    if (Session["role"].ToString() != "Admin")
        //    {
        //        return RedirectToAction("Index", "Unauthorized");
        //    }


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string SearchQuery = "SELECT * FROM dububase.premiumitem where itemName = @name";

        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
        //            c.Parameters.AddWithValue("@name",name);
        //            using (MySqlDataReader r = c.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    //wait
        //                    ViewBag.item = new PremiumItem
        //                    {
        //                        itemName = (r["itemName"]).ToString(),
        //                        itemType = (r["itemType"]).ToString(),
        //                        itemDescription = (r["itemDescription"]).ToString(),
               
                                
        //                        //itemImage = (r["itemImage"]).ToString()
        //                    };
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }
        //    return View("ItemManager");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ItemDelete(PremiumItem item)
        //{
        //    #region role and is logged in
        //    if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    if (Session["role"].ToString() != "Admin")
        //    {
        //        return RedirectToAction("Index", "Unauthorized");
        //    }


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string SearchQuery = "DELETE FROM dububase.itemName WHERE itemName= @itemName;";
        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
        //            c.Parameters.AddWithValue("@itemName", item.itemName);
        //            c.ExecuteNonQuery();
        //            return View("ItemManager");
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }
        //    return View();
        //}


        //public ActionResult ItemManager()
        //{
        //    #region role and is logged in
        //    //if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    //{
        //    //    return RedirectToAction("Login", "User");
        //    //}
        //    //Session["role"] = "Admin";
        //    //if (Session["role"].ToString() != "Admin")
        //    //{
        //    //    return RedirectToAction("Index", "Unauthorized");
        //    //}


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    List<PremiumItem> listy = new List<PremiumItem>();
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string SearchQuery = "Select * from dububase.premiumitem";
        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

        //            using (MySqlDataReader r = c.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    PremiumItem item = new PremiumItem
        //                    {
        //                        itemName = (r["itemName"]).ToString(),
        //                        beansPrice = Convert.ToInt32(r["beansPrice"].ToString()),
        //                        itemDescription = (r["itemDescription"]).ToString(),
        //                        itemType = (r["itemType"]).ToString()
        //                    };
        //                    listy.Add(item);
        //                }
        //            }
        //            ViewBag.Listy = listy;
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }

        //    return View();
        //}
        //[HttpGet]
        //public ActionResult ItemEdit(string name)
        //{
        //    #region role and is logged in
        //    //if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    //{
        //    //    return RedirectToAction("Login", "User");
        //    //}

        //    //if (Session["role"].ToString() != "Admin")
        //    //{

        //    //    return RedirectToAction("Index", "Unauthorized");
        //    //}


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            string SearchQuery = "Select * from dububase.premiumitem where itemName = @name";
        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
        //            c.Parameters.AddWithValue("@name", name);
        //            using (MySqlDataReader r = c.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    PremiumItem item = new PremiumItem
        //                    {
        //                        itemName = (r["itemName"]).ToString(),
        //                        beansPrice = Convert.ToInt32(r["beansPrice"].ToString()),
        //                        itemDescription = (r["itemDescription"]).ToString(),
        //                        itemType = (r["itemType"]).ToString()
        //                    };
        //                    ViewBag.item = item;
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }
        //    return View("ItemManager");
        //}
        ////Actual one
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ItemEdit(PremiumItem item)
        //{
        //    #region role and is logged in
        //    //if (Session["uname"] == null || Session["uname"].ToString() == "")
        //    //{
        //    //    return RedirectToAction("Login", "User");
        //    //}

        //    //if (Session["role"].ToString() != "Admin")
        //    //{

        //    //    return RedirectToAction("Index", "Unauthorized");
        //    //}


        //    #endregion
        //    Database d = Database.CurrentInstance;
        //    try
        //    {
        //        if (d.OpenConnection())
        //        {
        //            //update
        //            string SearchQuery = "Update dububase.premiumitem set itemPrice = @2, itemDescription = @3, itemType=@4 where itemName = @5";
        //            MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
        //            c.Parameters.AddWithValue("@2", item.priceOfBeans);
        //            c.Parameters.AddWithValue("@3", item.itemDescription);
        //            c.Parameters.AddWithValue("@4", item.itemType);
        //            c.Parameters.AddWithValue("@5", item.itemName);
        //            c.ExecuteNonQuery();

        //        }
        //    }
        //    catch (MySqlException e)
        //    {
        //        Debug.WriteLine("MySQL Error!");
        //    }
        //    finally
        //    {
        //        d.CloseConnection();
        //    }
        //    return View("ItemManager");
        //}
        public ActionResult UserProfile(string username)
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            if (Session["role"].ToString() != "Admin")
            {

                return RedirectToAction("Index", "Unauthorized");
            }
            Database d = Database.CurrentInstance;
            List<DummyProfile> Dummys = new List<DummyProfile>();
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.users Where username = @username;";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    c.Parameters.AddWithValue("@username", username);
                    AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
                    List<user> users = new List<user>();
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            user user = new user
                            {
                                userName = (r["userName"].ToString()),
                                email = (r["email"]).ToString(),
                                role = (r["role"].ToString())
                            };
                            users.Add(user);
                            ViewBag.Dummys = users;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
        }

    }
}
