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
using System.Diagnostics;

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

            var otp = 0;
            Random r1 = new Random();
            otp = r1.Next(1000, 100000);

            //to make it easier on me
            Debug.WriteLine(otp);

            var accountSid = ConfigurationManager.AppSettings["TwilioAccountsSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            //var to = new PhoneNumber(ConfigurationManager.AppSettings["MyPhoneNumber"]);
            var to = Session["Phonenumber"].ToString();
            var from = new PhoneNumber("+19104057634");

            try
            {
                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: "Hello from DubuUniverse! Your verification code number is: " + otp
                    );
                Content(message.Sid);
            } catch(Exception e)
            {
                //do nothing for now
            }

            HttpCookie otpCookie = new HttpCookie("OTP")
            {
                Value = otp.ToString()
            };
            
            otpCookie.Expires = DateTime.Now.AddMonths(2);
            Response.Cookies.Add(otpCookie);

            
            return RedirectToAction("SendOTP", "SMS");
        }

    }
}
