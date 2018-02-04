using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.IO;
using ASPJ_Project.Models;

namespace ASPJ_Project.Controllers
{
    public class SMSController : Controller
    {
       
        [HttpGet]
        public ActionResult SendOTP(string Phonenumber)
        {
           
            return View();
         
        }

        [HttpPost]
        public ActionResult SendOTP(string Email, UserLogin loginModel, string Username, string Phonenumber, string UserID)
        {

            var number = 0;
            Random r1 = new Random();
            number = r1.Next(1000, 100000);

            
            var accountSid = ConfigurationManager.AppSettings["TwilioAccountsSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            //var to = new PhoneNumber(ConfigurationManager.AppSettings["MyPhoneNumber"]);
            var to = Session["Phonenumber"].ToString();
            var from = new PhoneNumber("+19104057634");

            var message = MessageResource.Create(
                to: to,
                from: from,
                body: "Hello from DubuUniverse! Your verification code number is: " + number
                );
            return Content(message.Sid);

            HttpCookie otpCookie = new HttpCookie("OTP")
            {
                Value = otp.ToString()
            };
            
            otpCookie.Expires = DateTime.Now.AddMonths(3);
            Response.Cookies.Add(otpCookie);

            Content(message.Sid);
            
            return RedirectToAction("SendOTP", "SMS");

        }

    }
}
