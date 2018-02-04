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

namespace ASPJ_Project.Controllers
{
    public class SMSController : Controller
    {
       
        [HttpGet]
        public ActionResult SendOTP()
        {
            return View();
            

        }

        [HttpPost]
        public ActionResult SendOTP(string Email)
        {

            var number = 0;
            Random r1 = new Random();
            number = r1.Next(1000, 100000);

            //String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            //using (MySqlConnection con = new MySqlConnection(CS))
            //{
            //    con.Open();
            //    MySqlCommand cmd2 = new MySqlCommand("select * from accounts.users where email= @email", con);
            //    cmd2.Parameters.AddWithValue("email", Email);
            //    cmd2.ExecuteNonQuery();

            //    MySqlDataAdapter sda = new MySqlDataAdapter(cmd2);
            //    DataTable dt = new DataTable();
            //    sda.Fill(dt);

            //    if (dt.Rows.Count != 0)
            //    {
            //        int userID = Convert.ToInt32(dt.Rows[0][0]);
            //        MySqlCommand cmd3 = new MySqlCommand("INSERT INTO accounts.otp(otp, userID) VALUES(@otp, @userID)", con);

            //        cmd3.Parameters.AddWithValue("@otp", number);
            //        cmd3.Parameters.AddWithValue("@userID", userID);

            //        cmd3.ExecuteNonQuery();



            //    }
            //    con.Close();
            //}

            

            var accountSid = ConfigurationManager.AppSettings["TwilioAccountsSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(ConfigurationManager.AppSettings["MyPhoneNumber"]);
            var from = new PhoneNumber("+19104057634");

            var message = MessageResource.Create(
                to: to,
                from: from,
                body: "Hello from DubuUniverse! Your verification code number is: " + number
                );
            return Content(message.Sid);

            //HttpCookie otpCookie = new HttpCookie("OTP")
            //{
            //    Value = (Session["OTP"])
            //};


            //HttpCookie otpCookie = new HttpCookie("OTP");
            //otpCookie["OTP"] = otp.ToString();
            //otpCookie.Expires = DateTime.Now.AddMonths(3);
            //Response.Cookies.Add(otpCookie);

            HttpCookie otpCookie = new HttpCookie("OTP")
            {
                Value = otp.ToString()
            };
            
            otpCookie.Expires = DateTime.Now.AddMonths(3);
            Response.Cookies.Add(otpCookie);


            //Session["OTP"] = otp;

            Content(message.Sid);
            
            return RedirectToAction("SendOTP", "SMS");

            //string Username = ConfigurationManager.AppSettings["TwilioAccountsSid"];
            //string APIKey = ConfigurationManager.AppSettings["TwilioAuthToken"];
            //string SenderName = "Eunice";
            //string Number = ConfigurationManager.AppSettings["MyPhoneNumber"];
            //string Message = "Your otp ode is " + otp;
            //string URL = "https://api.twilio.in/send/?username=" + Username + "&hash=" + APIKey + "&sender=" +
            //    SenderName + "&numbers=" + Number + "&messsage=" + Message;


            //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //    StreamReader sr = new StreamReader(resp.GetResponseStream());
            //    string results = sr.ReadToEnd();
            //    sr.Close();

            //    

            //    return RedirectToAction("SendOTP", "SMS");






        }

    }
}
