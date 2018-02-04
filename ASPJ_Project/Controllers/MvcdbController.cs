using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using Censored;
using MySql.Data.MySqlClient;
using System.Collections;

namespace ASPJ_Project.Controllers
{
    public class MvcdbController : Controller
    {       
        public ActionResult Chat()
        {
            HttpCookie usernameCookie = new HttpCookie("UserID")
            {
                Value = HttpUtility.UrlEncode(AESCryptoStuff.CurrentInstance.AesEncrypt(""+Session["UserID"]))
            };
            Response.SetCookie(usernameCookie);
            DatabaseStuff db = new DatabaseStuff();
            ViewBag.dateTime = db.ChatGetTime();
            ViewBag.chatList = db.ChatGetMessage();
            return View();
        }      
    }

    public class EncodeDecode
    {
        public string EncodeStuff(string plaintext)
        {
            return HttpUtility.HtmlEncode(plaintext);
        }

        public string DecodeStuff(string encodedtext)
        {
            return HttpUtility.HtmlDecode(encodedtext);
        }
    }

    public class Censors
    {
        public string CrapCensor(string censorSomeWords)
        {
            var censoredWords = new List<string>
            { 
                "gosh",
                "drat",
                "darn*",
                "fuck",
                "anal",
                "anus",
                "arse",
                "ass",
                "ballsack",
                "balls",
                "bastard",
                "bitch",
                "biatch",
                "bloody",
                "blowjob",
                "blow job",
                "bollock",
                "bollok",
                "boner",
                "boob",
                "bugger",
                "bum",
                "butt",
                "buttplug",
                "clitoris",
                "cock",
                "coon",
                "crap",
                "cunt",
                "damn",
                "dick",
                "dildo",
                "dyke",
                "fag",
                "feck",
                "fellate",
                "fellatio",
                "felching",
                "fuck",
                "f u c k",
                "fudgepacker",
                "fudge packer",
                "flange",
                "Goddamn",
                "God damn",
                "hell",
                "homo",
                "jerk",
                "jizz",
                "knobend",
                "knob end",
                "labia",
                "lmfao",
                "muff",
                "nigger",
                "nigga",
                "omg",
                "penis",
                "piss",
                "poop",
                "prick",
                "pube",
                "pussy",
                "queer",
                "scrotum",
                "sex",
                "shit",
                "s hit",
                "sh1t",
                "slut",
                "smegma",
                "spunk",
                "tit",
                "tits",
                "tosser",
                "turd",
                "twat",
                "vagina",
                "wank",
                "whore",
                "wtf",
                "ji bai",
                "jibai",
                "na bei",
                "cb",
                "ccb",
                "kns",
                "mf",
                "motherfucker",
                "stupid",
                };
            var censor = new Censor(censoredWords);
            string result;
            result = censor.CensorText(censorSomeWords);
            return result;
        }
    }


    public class DatabaseStuff
    {
        //Declare db mysql stuff
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        AESCryptoStuff aes_obj = AESCryptoStuff.CurrentInstance;
        EncodeDecode encInit = new EncodeDecode();
        string queryString;

        //Insert message into the database
        public void ChatSendMessage(string chatMessageInsert)
        {
            Models.Database d = Models.Database.CurrentInstance;
            try
            { 
                //Initialize Command 
                MySqlCommand cmd = new MySqlCommand(queryString, d.conn);
                //Open connection
                d.OpenConnection();
                //Set date time
                DateTime date = DateTime.Now;
                string dateStr, timeStr, dateTimeStr;
                dateTimeStr = date.ToString("dd/MM/yyy hh:mm tt");
                dateStr = date.ToString("dd/MM/yyyy");
                timeStr = date.ToString("hh:mm tt");
                //Encode msg
                chatMessageInsert = encInit.EncodeStuff(chatMessageInsert);
                //Encrypt msg
                chatMessageInsert = aes_obj.AesEncrypt(chatMessageInsert);
                dateStr = aes_obj.AesEncrypt(dateStr);
                timeStr = aes_obj.AesEncrypt(timeStr);
                //Insert query
                queryString = "INSERT INTO dububase.chat(chatMessage, chatDate, chatTime) VALUES(@sendmessage, @chatdate, @chattime)";
                cmd.CommandText = queryString;
                //Add parameters
                cmd.Parameters.AddWithValue("@sendmessage", chatMessageInsert);
                cmd.Parameters.AddWithValue("@chatdate", dateStr);
                cmd.Parameters.AddWithValue("@chattime", timeStr);
                cmd.ExecuteNonQuery();
            }//Exception
            catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                //Close connection
                d.CloseConnection();
            }
        }

        //Retreive msg from db
        public List<string> ChatGetMessage()
        {
            //Set connection string
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            try
            {
                //Storing list
                List<string> chatList = new List<string>();
                List<string> decodedList = new List<string>();
                List<string> censoredList = new List<string>();
                //Open connection
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, conn);
                AESCryptoStuff aes_obj = AESCryptoStuff.CurrentInstance;
                Censors censorMessage = new Censors();
                //QueryString set
                queryString = "SELECT * FROM dububase.chat";
                cmd.CommandText = queryString;
                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
                reader = cmd.ExecuteReader();
                while (reader.HasRows && reader.Read())
                {
                    //While rows are present read and add each row from chatmessage column to chatList
                    chatList.Add(aes_obj.AesDecrypt(reader["chatMessage"].ToString()));
                }
                //Loop through list and decode
                foreach (string i in chatList)
                {
                    decodedList.Add(encInit.DecodeStuff(i));
                }
                foreach(string b in decodedList)
                {
                    censoredList.Add(censorMessage.CrapCensor(b));
                }
                return censoredList;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }           
        }

        //Retrieve time from db
        public List<string> ChatGetTime()
        {
            //string chatInfo;
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(queryString, conn);            
                List<string> storeDate = new List<string>();
                AESCryptoStuff aes_obj = AESCryptoStuff.CurrentInstance;
                queryString = "SELECT chatTime FROM dububase.chat";
                cmd.CommandText = queryString;
                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
                reader = cmd.ExecuteReader();
                while (reader.HasRows && reader.Read())
                {
                    storeDate.Add(aes_obj.AesDecrypt(reader["chatTime"].ToString()));
                }
                return storeDate;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
        }
    }    
}
