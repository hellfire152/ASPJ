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
        public ActionResult Index()
        {
            return View();
        }

        public string GetSalt()
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32];
            random.GetNonZeroBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public ActionResult ChangePassword()
        {
            Database d = Database.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dummyuser";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                        }
                    }
                }
            } catch (MySqlException e)
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
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LenbkIUAAAAAJGZh-mw37g7pIC-vLXNXAbxnsXd";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";
            //if (status == true)
            //{
            //    Database d = Database.CurrentInstance;
            //    try
            //    {
            //        if (d.OpenConnection(){
            //            string passwordQuery = "SELECT * FROM ACCOUNT";
            //            MySqlCommand c = new MySqlCommand(passwordQuery, d.conn);

            //            using (MySqlDataReader r = c.ExecuteReader())
            //            {
            //                while (r.Read())
            //                {
            //                    //Logic Much
            //                }
            //            }
            //        }
            //    }
            //    catch (MySQLException e)
            //    {
            //        Debug.WriteLine("MySQL Error!");
            //    }
            //    finally
            //    {
            //        d.CloseConnection();
            //    }
            //}
            return View("ChangePassword");
        }
        //public Boolean ChangePassword(string old, string newpass, string repass)
        //{
        //    //Logic: IF (hash of (old+salt)) == hash in database, and if new match renew, then set hash of (new +salt) to database
        //}

        public ActionResult UserProfile(string username)
        {
            //if(username != null)
            //{
            //    viewdata["user"] = db.users.find(username)
            //    return View();
            //}
            //userID,userName,password,firstName,lastName, email, phoneNumber,beansAmount
            DummyProfile dummy = new DummyProfile { Email="killme@html.com",FirstName="meh", Role = "Moderator", Username = "name" };

            ViewBag.dummy = dummy;
            return View();
        }
        public ActionResult TransactionHistory(string Username)
        {
            Session["username"] = "";
            if(Session["username"].ToString() != Username)
            {
                return RedirectToAction("TransactionHistory", "Profile", new { Username = Username });   
            }

            Database d = Database.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.beantransaction Order by dateOfTransaction Desc";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            //wait
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