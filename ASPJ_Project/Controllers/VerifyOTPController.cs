using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ASPJ_Project.Controllers
{
    public class VerifyOTPController : Controller
    {
        // GET: VerifyOTP
        [HttpPost]
        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyOTP(OTP otpModel, UserLogin loginModel, string Username)
        {
            //if (Session["OTP"].ToString() == otpModel.OTPvalue)

            if (Request.Cookies["OTP"].Value == otpModel.OTPvalue)
            {
                return RedirectToAction("UsersHome", "User");
            }
            else
            {
                ViewBag.Message = "Error";
                return RedirectToAction("SendOTP", "SMS");

            }



            //Session["OTP"] = null;

            //    try
            //    {
            //        String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            //        conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            //        conn.Open();

            //        MySqlCommand cmd = new MySqlCommand(queryStr, conn);
            //        queryStr = "SELECT * FROM accounts.users where email = @email and password = @password";

            //        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

            //        cmd.CommandText = queryStr;
            //        cmd.Parameters.AddWithValue("@email", loginModel.email);
            //        cmd.Parameters.AddWithValue("@password", loginModel.password);
            //        cmd.ExecuteNonQuery();

            //        reader = cmd.ExecuteReader();

            //        while (reader.HasRows && reader.Read())
            //        {
            //            Username = reader.GetString(reader.GetOrdinal("userName"));
            //        }
            //        if (reader.HasRows)
            //        {
            //            Session["UserID"] = Username;
            //            return RedirectToAction("UsersHome", "User");
            //        }

            //    }
            //    catch (System.Data.SqlClient.SqlException ex)
            //    {
            //        string errorMsg = "Error";
            //        errorMsg += ex.Message;
            //        throw new Exception(errorMsg);
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }

            //    return View();
            //}

        }
    }
}