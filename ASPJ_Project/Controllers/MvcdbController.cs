﻿using System;
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

namespace ASPJ_Project.Controllers
{
    public class MvcdbController : Controller
    {

        //private mvccruddbEntities dbModel = new mvccruddbEntities();
        // GET: Mvcdb
        //public ActionResult Index()
        //{
        //    List<user> userList = new List<user>();
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        IList<string> censoredWords = new List<string>
        //        {
        //          "gosh",
        //          "drat",
        //          "darn*",
        //          "fuck*",
        //          "idiot",
        //          "stupid",
        //          "ji bai",
        //          "bitch",
        //          "pussy",
        //          "shit",
        //          "cb",

        //        };

        //        //Censor censor = new Censor(censoredWords);
        //        //string result;

        //        //result = censor.CensorText("I stubbed my toe. Gosh it hurts!");
        //        //// I stubbed my toe. **** it hurts!

        //        //result = censor.CensorText("The midrate on the USD -> EUR forex trade has soured my day. Drat!");
        //        //// The midrate on the USD -> EUR forex trade has soured my day. ****!

        //        //result = censor.CensorText("Gosh darnit, my shoe laces are undone.");
        //        userList = dbModel.users.ToList<user>();
        //        //string censoredUserList;
        //        //foreach (var item in userList)
        //        //{
        //        //    censoredUserList = censor.CensorText(item.FirstName);
        //        //}

        //    }
        //    return View(userList);
        //}

        //public static void Encrypt(string value)
        //{
        //    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        //    {
        //        UTF8Encoding utf8 = new UTF8Encoding();
        //        byte[] data = md5.ComputeHash(utf8.GetBytes(value));
        //        Convert.ToBase64String(data);
        //    }

        //}

        // GET: Mvcdb/Details/5
        //public ActionResult Details(int id)
        //{
        //    user userModel = new user();
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
        //    }
        //    return View(userModel);
        //}

        // GET: Mvcdb/Create
        //public ActionResult Create()
        //{
        //    return View(new user());
        ////}

        // POST: Mvcdb/Create
        //[HttpPost]
        //public ActionResult Create(user userModel)
        //{            
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        //AESCryptoStuff aes_obj = new AESCryptoStuff();
        //        //userModel = aes_obj.AesEncrypt(userModel);
        //        dbModel.users.Add(userModel);
        //        dbModel.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        // GET: Mvcdb/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    user userModel = new user();
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
        //    }
        //    return View(userModel);
        //}

        // POST: Mvcdb/Edit/5
        //[HttpPost]
        //public ActionResult Edit(user userModel)
        //{
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        dbModel.Entry(userModel).State = System.Data.Entity.EntityState.Modified;
        //        dbModel.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        // GET: Mvcdb/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    user userModel = new user();
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
        //    }
        //    return View(userModel);
        //}

        // POST: Mvcdb/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        user userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
        //        dbModel.users.Remove(userModel);
        //        dbModel.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        //public ActionResult ChatTest()
        //{
        //    List<user> userList = new List<user>();
        //    using (mvccruddbEntities dbModel = new mvccruddbEntities())
        //    {
        //        userList = dbModel.users.ToList<user>();
        //    }

        //    return View(userList);
        //}

        //public ActionResult FriendList(string searchString)
        //{
        //    List<user> userList = new List<user>();
        //    var searchList = from c in dbModel.users select c;
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        //searchList = searchList.Where(c => c.FirstName == searchString);
        //        searchList = searchList.Where(c => c.FirstName.Contains(searchString));
        //    }
        //    //userList = dbModel.users.ToList<user>();

        //    return View(searchList);
        //}

        public ActionResult Chat()
        {
            //DatabaseStuff db = new DatabaseStuff();
            //db.ChatGetMessage();
            return View();
        }

        public static string Encode(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }

        public static string Decode(string encodedServername)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if( Request.QueryString["chatId"] != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine('Loading some chat');
        //        chatId = Request.QueryString["chatId"];

        //    }
        //}

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
                };
            var censor = new Censor(censoredWords);
            string result;
            result = censor.CensorText(censorSomeWords);
            return result;
        }
    }

    //public class Censored
    //{
    //    public string CensorStuff(string plaintext)
    //    {
    //        string censoredWord;
    //        List<string> censoredWordList = new List<string>();
    //        censoredWordList.Add("Fuck");
    //        censoredWord = plaintext.Replace(plaintext, "@$%^#!*&");
    //        Console.Write(censoredWord);

    //        return censoredWord;
    //    }
    //}

    public class DatabaseStuff
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        //MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        AESCryptoStuff aes_obj = new AESCryptoStuff();
        string queryString;

        //Insert message into the database
        public void ChatSendMessage(string chatMessageInsert)
        {
            //MySql.Data.MySqlClient.MySqlConnection conn;
            //MySql.Data.MySqlClient.MySqlCommand cmd;            
            try
            {
                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;                
                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();                
                MySqlCommand cmd = new MySqlCommand(queryString, conn);

                /*
                 command.Parameters.AddWithValue("@chatId", chatid);
                 if(userid == chatinfo.firstUserID.ToString()){
                    command.Parameters.AddWithValue("@senderUserID", chatinfo.firstuserID);
                    }
                    else{                                           
                    }
                  */
                aes_obj.AesInitialize();
                chatMessageInsert = aes_obj.AesEncrypt(chatMessageInsert);
                queryString = "INSERT INTO dububase.chat(chatMessage) VALUES(@sendmessage)";
                cmd.CommandText = queryString;
                cmd.Parameters.AddWithValue("@sendmessage", chatMessageInsert);
                cmd.ExecuteNonQuery();
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

        public void ChatGetMessage()
        {
            string chatInfo;
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ConnectionString;
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(queryString, conn);
            //AESCryptoStuff aes_obj = new AESCryptoStuff();
            //aes_obj.AesInitialize();
            //queryString = "";
            queryString = "SELECT * FROM dububase.chat";
            cmd.CommandText = queryString;
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            //cmd.Parameters.AddWithValue("@chatid", chatid);
            reader = cmd.ExecuteReader();
            while (reader.HasRows && reader.Read())
            {
                chatInfo = reader.GetString(reader.GetOrdinal("chatMessage"));
            }
            reader.Close();
            conn.Close();
        }
    }

    //class AesExample
    //{
    //    public static void Main()
    //    {
    //        try
    //        {

    //            string original = "Here is some data to encrypt!";

    //            // Create a new instance of the Aes
    //            // class.  This generates a new key and initialization 
    //            // vector (IV).
    //            using (Aes myAes = Aes.Create())
    //            {

    //                // Encrypt the string to an array of bytes.
    //                byte[] encrypted = EncryptStringToBytes_Aes(original,
    //                myAes.Key, myAes.IV);

    //                // Decrypt the bytes to a string.
    //                string roundtrip = DecryptStringFromBytes_Aes(encrypted,
    //                    myAes.Key, myAes.IV);

    //                //Display the original data and the decrypted data.
    //                Console.WriteLine("Original:   {0}", original);
    //                Console.WriteLine("Round Trip: {0}", roundtrip);
    //            }

    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("Error: {0}", e.Message);
    //        }
    //    }
    //    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)

    //    {
    //        // Check arguments.
    //        if (plainText == null || plainText.Length <= 0)
    //            throw new ArgumentNullException("plainText");
    //        if (Key == null || Key.Length <= 0)
    //            throw new ArgumentNullException("Key");
    //        if (IV == null || IV.Length <= 0)
    //            throw new ArgumentNullException("IV");
    //        byte[] encrypted;
    //        // Create an Aes object
    //        // with the specified key and IV.
    //        using (Aes aesAlg = Aes.Create())
    //        {
    //            aesAlg.Key = Key;
    //            aesAlg.IV = IV;

    //            // Create a decrytor to perform the stream transform.
    //            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


    //            // Create the streams used for encryption.
    //            using (MemoryStream msEncrypt = new MemoryStream())
    //            {
    //                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))

    //                {
    //                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))

    //                    {

    //                        //Write all data to the stream.
    //                        swEncrypt.Write(plainText);
    //                    }
    //                    encrypted = msEncrypt.ToArray();
    //                }
    //            }
    //        }


    //        // Return the encrypted bytes from the memory stream.
    //        return encrypted;

    //    }

    //    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)

    //    {
    //        // Check arguments.
    //        if (cipherText == null || cipherText.Length <= 0)
    //            throw new ArgumentNullException("cipherText");
    //        if (Key == null || Key.Length <= 0)
    //            throw new ArgumentNullException("Key");
    //        if (IV == null || IV.Length <= 0)
    //            throw new ArgumentNullException("IV");

    //        // Declare the string used to hold
    //        // the decrypted text.
    //        string plaintext = null;

    //        // Create an Aes object
    //        // with the specified key and IV.
    //        using (Aes aesAlg = Aes.Create())
    //        {
    //            aesAlg.Key = Key;
    //            aesAlg.IV = IV;

    //            // Create a decrytor to perform the stream transform.
    //            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);


    //            // Create the streams used for decryption.
    //            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
    //            {
    //                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))

    //                {
    //                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))

    //                    {

    //                        // Read the decrypted bytes from the decrypting 
    //                        //stream
    //                        //// and place them in a string.
    //                        //plaintext = srDecrypt.ReadToEnd();
    //                    }
    //                }
    //            }

    //        }

    //        return plaintext;

    //    }
    //}

    public class AESCryptoStuff
    {
        AesCryptoServiceProvider cryptProvider;
        public void AesInitialize ()
        {
            cryptProvider = new AesCryptoServiceProvider();

            cryptProvider.BlockSize = 128;
            cryptProvider.KeySize = 256;
            cryptProvider.GenerateIV();
            cryptProvider.GenerateKey();
            cryptProvider.Mode = CipherMode.CBC;
            cryptProvider.Padding = PaddingMode.PKCS7;
        }

        public String AesEncrypt(String clear_text)
        {
            ICryptoTransform transform = cryptProvider.CreateEncryptor();
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clear_text), 0, clear_text.Length);
            string str = Convert.ToBase64String(encrypted_bytes);
            return str;
        }

        public String AesDecrypt(String cipher_text)
        {
            ICryptoTransform transform = cryptProvider.CreateDecryptor();
            byte[] enc_bytes = Convert.FromBase64String(cipher_text);
            byte[] decrypted_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);
            string str = ASCIIEncoding.ASCII.GetString(decrypted_bytes);
            return str;
        }
    }

}