﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using CaptchaMvc.Attributes;

namespace ASPJ_Project.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {

        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        String queryStr;


        #region Registration w/ Hashing

        public ActionResult Registration()
        {
            return View();
        }

        //REGISTER USER
        [HttpGet]
        public ActionResult RegisterUser(int id = 0)
        {
            user userModel = new user();
            return View(userModel);
        }

        //CHECK IF USERNAME EXIST
        public JsonResult IsUsernameExists(user model)
        {
            String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(CS);
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(queryStr, conn);
            queryStr = "SELECT * FROM  users where userName= @userName";
            cmd1.CommandText = queryStr;
            cmd1.Parameters.AddWithValue("@userName", model.userName);
            cmd1.ExecuteNonQuery();

            reader = cmd1.ExecuteReader();
            if (reader.HasRows && reader.Read())
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

            //CHECK IF EMAIL EXIST
            public JsonResult IsEmailExists(user model)
            {
                String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
                conn = new MySql.Data.MySqlClient.MySqlConnection(CS);
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(queryStr, conn);
                queryStr = "SELECT * FROM users where email= @email";
                cmd1.CommandText = queryStr;
                cmd1.Parameters.AddWithValue("@email", model.email);
                cmd1.ExecuteNonQuery();

                reader = cmd1.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }


            }


        //REGISTER USER
        [HttpPost, CaptchaVerify("Captcha is not valid")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Registration(user model, string Username, string Password, string Firstname, string Lastname, string Email, string Phonenumber)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
                conn = new MySql.Data.MySqlClient.MySqlConnection(CS);
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(queryStr, conn);
                queryStr = "SELECT * FROM  users where email= @email";
                cmd1.CommandText = queryStr;
                cmd1.Parameters.AddWithValue("@email", Email);
                cmd1.ExecuteNonQuery();

                reader = cmd1.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    ViewBag.Message = "Email is already been used. Please use another email.";
                    conn.Close();
                    return View();
                    
                }
                else
                {
                    try
                    {
                        string ActivationCode = Guid.NewGuid().ToString();
                        
                        #region Password Hashing
                        
                        model.password = AESCryptoStuff.CurrentInstance.AesEncrypt(Crypto.Hash(model.confirmPassword));
                        model.confirmPassword = AESCryptoStuff.CurrentInstance.AesEncrypt(Crypto.Hash(model.confirmPassword));

                        #endregion
                        model.phoneNumber = AESCryptoStuff.CurrentInstance.AesEncrypt(model.phoneNumber);
                        //String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
                        conn = new MySql.Data.MySqlClient.MySqlConnection(CS);
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(queryStr, conn);
                        queryStr = "INSERT INTO  users(userName, password, firstName, lastName, email, phoneNumber) VALUES(@userName, @password, @firstName, @lastName, @email, @phoneNumber)";
                        cmd.CommandText = queryStr;
                        cmd.Parameters.AddWithValue("@userName", Username);
                        cmd.Parameters.AddWithValue("@password", model.password);
                        cmd.Parameters.AddWithValue("@firstName", Firstname);
                        cmd.Parameters.AddWithValue("@lastName", Lastname);
                        cmd.Parameters.AddWithValue("@email", Email);
                        cmd.Parameters.AddWithValue("@phoneNumber", model.phoneNumber);

                        cmd.ExecuteNonQuery();

                        conn.Close();
                        using (MySqlConnection con = new MySqlConnection(CS))
                        {
                            con.Open();
                            MySqlCommand cmd2 = new MySqlCommand("select * from  users where email= @email", con);
                            cmd2.Parameters.AddWithValue("email", Email);

                            MySqlDataAdapter sda = new MySqlDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count != 0)
                            {
                                int userID = Convert.ToInt32(dt.Rows[0][0]);
                                MySqlCommand cmd3 = new MySqlCommand("insert into  activation(activationCode, userID) VALUES (@activationCode, @userID)", con);

                                cmd3.Parameters.AddWithValue("@activationCode", ActivationCode);
                                cmd3.Parameters.AddWithValue("@userID", userID);
                                cmd3.ExecuteNonQuery();

                                //EMAIL USER ACTIVATION LINK

                                var verifyUrl = "/User/VerifyAccount" + "/?DubuID=" + ActivationCode;

                                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                                String ToEmailAddress = dt.Rows[0][5].ToString();
                                Firstname = dt.Rows[0][3].ToString();
                                String EmailBody = "Hi, " + Firstname + ",<br/><br/>Thank you for registering with us.<br/>One more step... <br/> " +
                                    "Click the link below to continue.<br/>" +
                                    "<a href='" + link + "'>" + link + "</a><br/>" +
                                    "Thank you, <br/>Team Dubu Universe.";
                                MailMessage PassRecMail = new MailMessage("\"Dubu Universe \" <dubuuniverse@gmail.com>", ToEmailAddress);
                                PassRecMail.Body = EmailBody;
                                PassRecMail.IsBodyHtml = true;
                                PassRecMail.Subject = "Your account has been created!";

                                SmtpClient SMTP = new SmtpClient("smtp.gmail.com", 25);
                                SMTP.Credentials = new NetworkCredential()
                                {
                                    UserName = ConfigurationManager.AppSettings["fromEmailAddress"],
                                    Password = ConfigurationManager.AppSettings["fromEmailPassword"]
                                    //UserName = "dubuuniverse@gmail.com",
                                    //Password = "P@ssw0rd123"


                                };

                                SMTP.EnableSsl = true;
                                SMTP.Send(PassRecMail);

                                Response.Write("<script> alert('Thank you for registering, Please proceed to login.')</script>");
                                return RedirectToAction("Login", "User");

                            }
                        }
                        conn.Close();

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

            }
            else
            {
                message = "Invalid Request, ";
                //ViewBag.ErrorMessage = "Incorrect Captcha";
            }
            ViewBag.message = message;
            ViewBag.Status = Status;

            return View(model);
        }

        #endregion
        
        #region Verify Account
        [HttpPost]
        public ActionResult VerifyAccount()
        {
            
            return View();

        }

        //VERIFY ACCOUNT
        [HttpGet]
        public ActionResult VerifyAccount(string activationCode)
        {
            String CS = ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            int Uid = 0;

            //CHECK IF ENTRY IS VALID 
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                con.Open();
                activationCode = Request.QueryString["DubuID"];

                if (activationCode != null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from  activation where activationCode= @activationCode", con);
                    cmd.Parameters.AddWithValue("@activationCode", activationCode);
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    //Debug.WriteLine("1");
                    sda.Fill(dt);
                    //Debug.WriteLine(dt.Rows[0][1]);
                    if (dt.Rows.Count != 0)
                    {
                        Uid = Convert.ToInt32(dt.Rows[0][1]);
                        //Debug.WriteLine("2");
                        
                    }
                    else
                    {

                        return RedirectToAction("Login", "User");

                    }

                    MySqlCommand cmd3 = new MySqlCommand("delete from  activation where userID = @userID", con);
                    cmd3.Parameters.AddWithValue("userID", Uid);
                    cmd3.ExecuteNonQuery();
                    con.Close();
                    return RedirectToAction("VerifiedAccount", "User");
                   
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
                //ViewBag.Message = "Hello";
            }

        }

        public ActionResult VerifiedAccount()
        {
            return View();
        }

        #endregion

        #region Login
        //LOGIN
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string Username, string Phonenumber, string beansAmount, string returnUrl = "")
        {
            //string Username;
            int Uid = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
                    conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(queryStr, conn);
                    queryStr = "SELECT * FROM  users where email = @email and password = @password";

                    cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

                    cmd.CommandText = queryStr;
                    cmd.Parameters.AddWithValue("@email", login.email);
                    cmd.Parameters.AddWithValue("@password", AESCryptoStuff.CurrentInstance.AesEncrypt(Crypto.Hash(login.password)));
                    cmd.ExecuteNonQuery();

                    reader = cmd.ExecuteReader();
                    string role = "";
                    string isBan = "false";
                    string banTill = "";
                    string id = "";
                    while (reader.HasRows && reader.Read())
                    {
                        isBan = reader.GetString(reader.GetOrdinal("isBan"));
                        role = reader.GetString(reader.GetOrdinal("role"));
                        Uid = reader.GetInt32(reader.GetOrdinal("userID"));
                        Username = reader.GetString(reader.GetOrdinal("userName"));
                        login.email = reader.GetString(reader.GetOrdinal("email"));
                        login.password = reader.GetString(reader.GetOrdinal("password"));
                        Phonenumber = AESCryptoStuff.CurrentInstance.AesDecrypt(reader.GetString(reader.GetOrdinal("phoneNumber")));
                        beansAmount = reader.GetString(reader.GetOrdinal("beansAmount"));
                        if (reader["banTill"] != DBNull.Value)
                        {
                            banTill = reader.GetString(reader.GetOrdinal("banTill"));
                        }
                        if (reader["banID"] != DBNull.Value)
                        {
                            id = reader.GetString(reader.GetOrdinal("banID"));
                        }
                    }
                    if (isBan == "true")
                    {
                        TempData["username"] = Username;
                        TempData["banID"] = id;
                        TempData["banTill"] = banTill;
                        return RedirectToAction("Banned", "Unauthorised");
                    }
                    if (reader.HasRows)
                    {
                        String CS = ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
                        using (MySqlConnection con = new MySqlConnection(CS))
                        {

                            MySqlCommand cmd2 = new MySqlCommand("select * from  activation where userID = @userID", con);

                            cmd2.Parameters.AddWithValue("userID", Uid);

                            con.Open();
                            MySqlDataAdapter sda = new MySqlDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {

                                int timeout = login.RememberMe ? 525600 : 20; //525600 mins = 1 year
                                var ticket = new FormsAuthenticationTicket(login.email, login.RememberMe, timeout);
                                string encrypted = FormsAuthentication.Encrypt(ticket);
                                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                                cookie.HttpOnly = true; //no javascript allowed
                                Response.Cookies.Add(cookie);

                                if (Url.IsLocalUrl(returnUrl))
                                {
                                    return Redirect(returnUrl);

                                }
                                else
                                {
                                   
                                    Session["tuname"] = Username;
                                    Session["tuserID"] = Uid;
                                    Session["tPhonenumber"] = Phonenumber;
                                    Session["tusername"] = Username;
                                    Session["tuserBeans"] = beansAmount;
                                    Session["trole"] = role;
                                    return RedirectToAction("SendOTP", "SMS");

                                }
                                
                            }
                            else
                            {
                                ViewBag.Message = "You have not activated your account. Please check your email for activation link.";
                                return View();
                            }

                        }

                    }
                    else
                    {
                        ViewBag.Message = "Invalid User. Please check your username and password again.";
                        return View();
                    }

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

            return View();
        }

      

            #endregion

            #region Testing Out User home page after login

            public ActionResult UsersHome()
        {
            return View();
        }

        #endregion

        #region Log Out
        //LOGOUT
       
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        #endregion

     
        //Done
        #region Forget password, input email for reset password link


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            //VERIFY VALID EMAIL
            //GENERATE RESET PASSWORD LINK
            //SEND EMAIL
            DateTime datetime = DateTime.Now;

            String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                //CHECK IF EMAIL EXIST IN DATABASE
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from  users where email = @email", con);
                cmd.Parameters.AddWithValue("email", Email);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    //INSERT REQUEST INTO DATABASE
                    String myGUID = Guid.NewGuid().ToString();
                    int userID = Convert.ToInt32(dt.Rows[0][0]);
                    MySqlCommand cmd1 = new MySqlCommand("insert into  resetpasswordrequest(ID, userID, resetRequestDateTime) values(@ID, @userID, @resetRequestDateTime)", con);
                    cmd1.Parameters.AddWithValue("@ID", myGUID);
                    cmd1.Parameters.AddWithValue("userID", userID);
                    cmd1.Parameters.AddWithValue("@resetRequestDateTime", datetime);

                    cmd1.ExecuteNonQuery();

                    //EMAIL USER ACTIVATION LINK

                    var verifyUrl = "/User/ResetPassword" + "/?DubuID=" + myGUID;

                    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                    String ToEmailAddress = dt.Rows[0][5].ToString();
                    String Firstname = dt.Rows[0][3].ToString();
                    String EmailBody = "Hi, " + Firstname + ",<br/><br/>We received your reset password request.<br/>" +
                        "To continue, <br/> " +
                        "Click the link below to proceed to reset your password.<br/>" +
                        "<a href='" + link + "'>" + link + "</a><br/>" +
                        "Thank you, <br/>Team Dubu Universe.";
                    MailMessage PassRecMail = new MailMessage("\"Dubu Universe \" <dubuuniverse@gmail.com>", ToEmailAddress);
                    PassRecMail.Body = EmailBody;
                    PassRecMail.IsBodyHtml = true;
                    PassRecMail.Subject = "Reset Password";

                    SmtpClient SMTP = new SmtpClient("smtp.gmail.com", 25);
                    SMTP.Credentials = new NetworkCredential()
                    {
                        UserName = ConfigurationManager.AppSettings["fromEmailAddress"],
                        Password = ConfigurationManager.AppSettings["fromEmailPassword"]
                        //UserName = "dubuuniverse@gmail.com",
                        //Password = "P@ssw0rd123"


                    };

                    SMTP.EnableSsl = true;
                    SMTP.Send(PassRecMail);

                    ViewBag.Message = "Please check your email to proceed.";
                }
                else
                {
                    ViewBag.Message = "Account not found.";
                }
                con.Close();
            }


            return View();

        }
        #endregion

        //not done
        #region Reset password, reset code

        //[HttpPost]
        //public ActionResult ResetPassword(string ID, ResetPassword resetpwModel, string Email)
        //{
        //    String CS = ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
        //    using (MySqlConnection con = new MySqlConnection(CS))
        //    {
        //        //UPDATE PASSWORD TO DATEBASE
        //        MySqlCommand cmd2 = new MySqlCommand("update  users set password = @password  where userID= @userID", con);
        //        cmd2.Parameters.AddWithValue("@password", resetpwModel.NewPassword);
        //        cmd2.Parameters.AddWithValue("@userID", Uid);
        //        cmd2.ExecuteNonQuery();

        //        MySqlCommand cmd3 = new MySqlCommand("delete from  resetpasswordrequest where userID = @userID", con);
        //        cmd3.Parameters.AddWithValue("userID", Uid);
        //        cmd3.ExecuteNonQuery();

        //        con.Close();

        //        ViewBag.Message = "Password Updated, Please proceed to login with your new password.";


        //    }
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult ResetPassword(string ID)
        //{
        //    ID = Request.QueryString["DubuID"];
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult ResetPassword()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult ResetPassword(string myGUID, ResetPassword resetpwModel, string Email)
        {
            String CS = ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            int Uid = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(CS))
                    {
                        con.Open();
                        myGUID = Request.QueryString["DubuID"];

                        if (myGUID != null)
                        {
                            MySqlCommand cmd = new MySqlCommand("select * from  resetpasswordrequest where ID= @ID", con);
                            cmd.Parameters.AddWithValue("@ID", myGUID);
                            cmd.ExecuteNonQuery();

                            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count != 0)
                            {
                                Debug.WriteLine("2");
                                Uid = Convert.ToInt32(dt.Rows[0][1]);

                            }
                            else
                            {
                                //ViewBag.Message = "Your Reset Link is Expired or Invalid.";
                                //return View();
                                //HIDE page away
                                return RedirectToAction("Login", "User");

                            }

                            
                            //UPDATE PASSWORD TO DATEBASE
                            MySqlCommand cmd2 = new MySqlCommand("update  users set password = @password  where userID= @userID", con);
                            cmd2.Parameters.AddWithValue("@password", resetpwModel.NewPassword);
                            cmd2.Parameters.AddWithValue("@userID", Uid);
                            cmd2.ExecuteNonQuery();

                            MySqlCommand cmd3 = new MySqlCommand("delete from  resetpasswordrequest where userID = @userID", con);
                            cmd3.Parameters.AddWithValue("userID", Uid);
                            cmd3.ExecuteNonQuery();


                            //MAIL USER AFTER PASSWORD UPDATED
                            MySqlCommand cmd4 = new MySqlCommand("select * from  users where email = @email", con);
                            cmd.Parameters.AddWithValue("email", Email);
                            cmd.ExecuteNonQuery();

                            MySqlDataAdapter sda2 = new MySqlDataAdapter(cmd);
                            DataTable dt2 = new DataTable();
                            sda.Fill(dt2);

                            if (dt2.Rows.Count != 0)
                            {
                                String ToEmailAddress = dt2.Rows[0][5].ToString();
                                String Firstname = dt2.Rows[0][3].ToString();
                                String EmailBody = "Hi, " + Firstname + ",<br/>" +
                                        "<br/>Your password have been reset successfully.<br/>" +
                                    "Please contact us immediately if it is not done by you.<br/> " +
                                    "Thank you, <br/>Team Dubu Universe.";
                                MailMessage PassRecMail = new MailMessage("\"Dubu Universe \" <dubuuniverse@gmail.com>", ToEmailAddress);
                                PassRecMail.Body = EmailBody;
                                PassRecMail.IsBodyHtml = true;
                                PassRecMail.Subject = "Reset Password";

                                SmtpClient SMTP = new SmtpClient("smtp.gmail.com", 25);
                                SMTP.Credentials = new NetworkCredential()
                                {
                                    UserName = ConfigurationManager.AppSettings["fromEmailAddress"],
                                    Password = ConfigurationManager.AppSettings["fromEmailPassword"]
                                    //UserName = "dubuuniverse@gmail.com",
                                    //Password = "P@ssw0rd123"


                                };

                                SMTP.EnableSsl = true;
                                SMTP.Send(PassRecMail);

                                con.Close();
                            }
                            else
                            {
                                return RedirectToAction("Login", "User");
                            }

                        }

                    }
                   
                }

                catch (System.Data.SqlClient.SqlException ex)
                {
                    string errorMsg = "Error";
                    errorMsg += ex.Message;
                    throw new Exception(errorMsg);
                }
                
            }
            return View();
        }


        [HttpGet]
        public ActionResult ResetPassword(string myGUID)
        {

            String CS = ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            int Uid = 0;

            //CHECK IF ENTRY IS VALID 
            using (MySqlConnection con = new MySqlConnection(CS))
            {
                con.Open();
                myGUID = Request.QueryString["DubuID"];

                if (myGUID != null)
                {
                    MySqlCommand cmd = new MySqlCommand("select * from  resetpasswordrequest where ID= @ID", con);
                    cmd.Parameters.AddWithValue("@ID", myGUID);
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count != 0)
                    {
                        Uid = Convert.ToInt32(dt.Rows[0][1]);

                    }
                    else
                    {
                        //ViewBag.Message = "Your Reset Link is Expired or Invalid.";
                        ////HIDE page away
                        return RedirectToAction("Login", "User");

                    }


                    con.Close();

                    ViewBag.Message = "Password Updated, Please proceed to login with your new password.";




                }

                else
                {
                    return RedirectToAction("Login", "User");


                }

            }
            return View();
        }






        #endregion

        #region MVC reset password
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResetPassword(ResetPassword model)
        //{
        //    var message = "";
        //    if (ModelState.IsValid)
        //    {
        //        //using (accountsEntities2 db = new accountsEntities2())
        //        //{
        //        //    var user = db.users.Where(a => a.resetPasswordID == model.ResetCode).FirstOrDefault();
        //        //    if(user != null)
        //        //    {
        //        //        user.password = Crypto.Hash(model.NewPassword);
        //        //        user.resetPasswordID = ""; //ONLY VALID ONCE

        //        //        db.Configuration.ValidateOnSaveEnabled = false;
        //        //        db.SaveChanges();
        //        //        message = "New password updated successfully";
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        message = "Invalid";
        //    }

        //    ViewBag.Message = message;
        //    return View(model);
        //}

        #endregion


    }
}


