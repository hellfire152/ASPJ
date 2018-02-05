using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using ASPJ_Project.Models;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace ASPJ_Project.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
      
        public string GetSalt()
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];
            random.GetNonZeroBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public ActionResult ChangePassword()
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {

            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LenbkIUAAAAAJGZh-mw37g7pIC-vLXNXAbxnsXd";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

            if (status == true)
            {
                Database d = Database.CurrentInstance;
                AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
                try
                {
                    if (d.OpenConnection())
                    {
                        var username = Session["uName"];
                        string SearchQuery = "SELECT * FROM dububase.users Where @username";


                        MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                        c.Parameters.AddWithValue("@username", username);
                        string password = "";
                        using (MySqlDataReader r = c.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                password = AES.AesDecrypt(r["password"].ToString());
                            }
                        }
                        var OldPassword = Crypto.Hash(model.OldPassword);
                        var NewPassword = Crypto.Hash(model.NewPassword);
                        if (OldPassword == password)
                        {
                            string query = "Update dububase.users set password = @newPassword where username =@username;";
                            c = new MySqlCommand(query, d.conn);
                            c.Parameters.AddWithValue("@newPassword", AES.AesEncrypt(NewPassword));
                            c.Parameters.AddWithValue("@username", username);
                            c.BeginExecuteNonQuery();
                            return View("Profile");
                        }
                        else
                        {
                            ViewBag.Message = "Wrong Password";
                            return View();
                        };

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
            }
            return View();
        }
        //public Boolean ChangePassword(string old, string newpass, string repass)
        //{
        //    //Logic: IF (hash of (old+salt)) == hash in database, and if new match renew, then set hash of (new +salt) to database
        //}

        
        public ActionResult UserProfile()
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var username = Session["uname"].ToString();
            Database d = Database.CurrentInstance;
            List<DummyProfile> Dummys = new List<DummyProfile>();
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.users Where userName = @username;";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    c.Parameters.AddWithValue("@username",username);
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
                                firstName = (r["firstName"].ToString()),
                                lastName = (r["lastName"].ToString()),
                                phoneNumber = (AES.AesDecrypt(r["phoneNumber"].ToString()))
                            };
                            
                            ViewBag.Dummys = user;
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
        public ActionResult TransactionHistory()
        {
            if (Session["uname"] == null || Session["uname"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            var Username = Session["uname"];

            //if(Session["u"].ToString() != Username)
            //{
            //    return RedirectToAction("TransactionHistory", "Profile", new { Username = Username });   
            //}
            
            Database d = Database.CurrentInstance;

            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.beantransaction Where userID = @userID Order by dateOfTransaction Desc";
                    //string SearchQuery = "Select userID From dububase.users where username = @username;";
                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    //c.Parameters.AddWithValue("@username", Username);
                    //int userID = 0;
                    //using (MySqlDataReader r = c.ExecuteReader())
                    //{
                    //    while (r.Read())
                    //    {
                    //        userID = Convert.ToInt32(r["userID"].ToString());
                    //    }
                    //}
                    var uid = Session["userID"].ToString();
                    
                    c = new MySqlCommand(SearchQuery, d.conn);
                    AESCryptoStuff AES = AESCryptoStuff.CurrentInstance;
                    c.Parameters.AddWithValue("@userID", AES.AesEncrypt(uid.ToString()));
                    List<TransactionHistory> transactions = new List<TransactionHistory>();

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            transactions.Add(new TransactionHistory
                            {
                                TransactionNo = AES.AesDecrypt(r["transactionNo"].ToString()),
                                TransactionDesc = AES.AesDecrypt(r["transactionDesc"].ToString()),
                                Price = Convert.ToDouble(r["priceOfBeans"]),
                                Status = r["status"].ToString(),
                                BeansBefore = Convert.ToInt32(r["userBeansBefore"]),
                                BeansAfter = Convert.ToInt32(r["userBeansAfter"]),
                                DateOfTransaction = (Convert.ToDateTime(r["dateOfTransaction"])).ToString(),
                                UserID = AES.AesDecrypt(r["UserID"].ToString())
                            });
                        }
                    }
                    ViewBag.Transactions = transactions;
                    return View();
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
    }
}