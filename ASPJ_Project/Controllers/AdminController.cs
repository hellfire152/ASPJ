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
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }
        //public ActionResult ViewUser(DummyProfile dummy)
        //{
        //    DummyProfile user = dummy;
        //    ViewData["role"] = user.role;
        //    if (dummy.role == "Admin")
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToRoute("action","index");
        //    }
        //}
        
        public ActionResult TransactionHistory()
        {
            Database d = Database.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.beantransaction";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                           //get item lah fk me
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
            //if(Session["role"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            //if user is ban
            //Session["isBan"] = "true";
            //string isBan = Session["isBan"].ToString();
            //if (isBan == "true")
            //{
            //    //get the banTill Period and compare if it is over than set isBan to "false"
            //    //TempData["username"] = get from eunice page database side lololol
            //    // do this at the banned details page
            //    //TempData.Clear()
            //    //redirect to ban page with reason maybe?
            //    TempData["BanTill"] = "MyMessage" //get from db the banTill;
            //    //use at the other page
            //    //ViewBag.Message = TempData["shortMessage"].ToString();
            //    return RedirectToAction("", "");
            //}
            #endregion

            Database d = Database.CurrentInstance;
            List<DummyProfile> Dummys = new List<DummyProfile>();

            try
            {
              if (d.OpenConnection())
              {
                    string SearchQuery = "SELECT * FROM dububase.users";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            DummyProfile dummy = new DummyProfile
                            {
                                Id = (int.Parse(r["UserId"].ToString())),
                                Username = (r["UserName"].ToString()),
                                FirstName = (r["FirstName"]).ToString(),
                                Role = (r["Role"].ToString()),
                                Email = (r["Email"].ToString())
                            };
                            Dummys.Add(dummy);
                            Debug.WriteLine(Dummys);
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

            #region db
            Database d = Database.CurrentInstance;

            List<DummyProfile> Dummys = new List<DummyProfile>();


            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dummyuser WHERE username LIKE %" + model.Search + ";";
                    //d.PNQ("SELECT * FROM dububase.users WHERE @searchtype LIKE @search%",model.SearchType, model.Search);
                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            DummyProfile dummy = new DummyProfile
                            {
                                Id = (int.Parse(r["Id"].ToString())),
                                Username = (r["username"].ToString()),
                                Role = (r["role"].ToString()),
                                Email = (r["email"].ToString()),
                                Status = (r["status"].ToString())
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
            #endregion

        }


        public ActionResult BanUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BanUser(BanUserModel model)
        {
            string username = model.Username;
            //db stuff
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            AESCryptoStuff aes_obj = new AESCryptoStuff();
            //EncodeDecode encInit = new EncodeDecode();
            Debug.WriteLine("this is model.banPeriod"+model.BanPeriod);
            try
            {
                string queryString = "";
                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                aes_obj.AesInitialize();
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
                //queryString = "UPDATE dububase.users SET isBan = 'true', banTill=@date Where username=@username;";
                //cmd.CommandText = queryString;
                //DateTime mehgofu = DateTime.Now.AddDays(model.BanPeriod);
                //cmd.Parameters.AddWithValue("@date", mehgofu);
                //cmd.Parameters.AddWithValue("@username", model.Username);
                //cmd.ExecuteNonQuery();

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
            return View();
        }
        

        public ActionResult BanHistory(BanSearchModel model)
        {
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
        
        public ActionResult RoleChange(ChangeRoleModel model)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
            AESCryptoStuff aes_obj = new AESCryptoStuff();
            try
            {
                string queryString = "";
                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                queryString = "UPDATE dububase.users SET role = @role Where username=@username;";
                cmd.CommandText = queryString;
                cmd.Parameters.AddWithValue("@role", model.NewRole);
                cmd.Parameters.AddWithValue("@username", model.Username);
                cmd.ExecuteNonQuery();

            }catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                conn.Close();
            }

            //change to somewhere
            return View();
        }
    }
}
