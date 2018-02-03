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
        public ActionResult VerifyOTP(OTP otpModel)
        {

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            //        conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            //        conn.Open();

            //        MySqlCommand cmd = new MySqlCommand(queryStr, conn);
            //        queryStr = "SELECT * FROM accounts.users where email = @email and password = @password";

            //        cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

            //        cmd.CommandText = queryStr;
            //        cmd.Parameters.AddWithValue("@email", login.email);
            //        cmd.Parameters.AddWithValue("@password", login.password);
            //        cmd.ExecuteNonQuery();

            //        reader = cmd.ExecuteReader();

            //        while (reader.HasRows && reader.Read())
            //        {
            //            Uid = reader.GetInt32(reader.GetOrdinal("userID"));
            //            Username = reader.GetString(reader.GetOrdinal("userName"));
            //            login.email = reader.GetString(reader.GetOrdinal("email"));
            //            login.password = reader.GetString(reader.GetOrdinal("password"));
            //            Phonenumber = reader.GetString(reader.GetOrdinal("phoneNumber"));
            //        }
            //        String CS = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
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




            return View();
        }
    }
}