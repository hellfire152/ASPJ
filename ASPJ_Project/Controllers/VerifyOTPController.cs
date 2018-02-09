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

        [HttpPost]
        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyOTP(OTP otpModel)
        {
            if (Request.Cookies["OTP"].Value == otpModel.OTPvalue)
            {
                Session["uname"] = Session["tuname"];
                Session["userID"] = Session["tuserID"];
                Session["Phonenumber"] = Session["tPhonenumber"];
                Session["username"] = Session["tusername"];
                Session["userBeans"] = Session["tuserBeans"];
                Session["role"] = Session["trole"];
                return RedirectToAction("Index", "Game");
            }
            else
            {
                
                return RedirectToAction("SendOTP", "SMS");
              
            }

        }
    }
}