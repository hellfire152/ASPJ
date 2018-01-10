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

namespace ASPJ_Project.Controllers
{
    public class MvcdbController : Controller
    {

        private mvccruddbEntities dbModel = new mvccruddbEntities();
        // GET: Mvcdb
        public ActionResult Index()
        {
            List<user> userList = new List<user>();
            using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                IList<string> censoredWords = new List<string>
                {
                  "gosh",
                  "drat",
                  "darn*",
                  "fuck*",
                  "idiot",
                  "stupid",
                  "ji bai",
                  "bitch",
                  "pussy",
                  "shit",
                  "cb",

                };

                Censor censor = new Censor(censoredWords);
                //string result;

                //result = censor.CensorText("I stubbed my toe. Gosh it hurts!");
                //// I stubbed my toe. **** it hurts!

                //result = censor.CensorText("The midrate on the USD -> EUR forex trade has soured my day. Drat!");
                //// The midrate on the USD -> EUR forex trade has soured my day. ****!

                //result = censor.CensorText("Gosh darnit, my shoe laces are undone.");
                userList = dbModel.users.ToList<user>();
                string censoredUserList;
                foreach(var item in userList)
                {
                  censoredUserList = censor.CensorText(item.FirstName);
                }
                
            }
            return View(userList);
        }

        public static void Encrypt(string value)
        {
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                Convert.ToBase64String(data);
            }

        }

        // GET: Mvcdb/Details/5
        public ActionResult Details(int id)
        {
            user userModel = new user();
            using (mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
            }
            return View(userModel);
        }

        // GET: Mvcdb/Create
        public ActionResult Create()
        {
            return View(new user());
        }

        // POST: Mvcdb/Create
        [HttpPost]
        public ActionResult Create(user userModel)
        {
            using (mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                dbModel.users.Add(userModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Mvcdb/Edit/5
        public ActionResult Edit(int id)
        {
            user userModel = new user();
            using (mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
            }
            return View(userModel);
        }

        // POST: Mvcdb/Edit/5
        [HttpPost]
        public ActionResult Edit(user userModel)
        {
           using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                dbModel.Entry(userModel).State = System.Data.Entity.EntityState.Modified;
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Mvcdb/Delete/5
        public ActionResult Delete(int id)
        {
            user userModel = new user();
            using (mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
            }
            return View(userModel);
        }

        // POST: Mvcdb/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
           using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                user userModel = dbModel.users.Where(x => x.UserID == id).FirstOrDefault();
                dbModel.users.Remove(userModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChatTest()
        {
            List<user> userList = new List<user>();
            using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                userList = dbModel.users.ToList<user>();
            }

            return View(userList);
        }

        public ActionResult FriendList(string searchString)
        {
            List<user> userList = new List<user>();
            var searchList = from c in dbModel.users select c;
                if (!string.IsNullOrEmpty(searchString))
                {
                    //searchList = searchList.Where(c => c.FirstName == searchString);
                    searchList = searchList.Where(c => c.FirstName.Contains(searchString));
                }
                //userList = dbModel.users.ToList<user>();

            return View(searchList);
        }

        public ActionResult Chat()
        {
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
    }
    public class Censor
    {
        public IList<string> CensoredWords { get; private set; }

        public Censor(IEnumerable<string> censoredWords)
        {
            if (censoredWords == null)
                throw new ArgumentNullException("censoredWords");

            CensoredWords = new List<string>(censoredWords);
        }

        public string CensorText(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            string censoredText = text;

            foreach (string censoredWord in CensoredWords)
            {
                string regularExpression = ToRegexPattern(censoredWord);

                censoredText = Regex.Replace(censoredText, regularExpression, StarCensoredMatch,
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return censoredText;
        }

        private static string StarCensoredMatch(Match m)
        {
            string word = m.Captures[0].Value;

            return new string('*', word.Length);
        }

        private string ToRegexPattern(string wildcardSearch)
        {
            string regexPattern = Regex.Escape(wildcardSearch);

            regexPattern = regexPattern.Replace(@"\*", ".*?");
            regexPattern = regexPattern.Replace(@"\?", ".");

            if (regexPattern.StartsWith(".*?"))
            {
                regexPattern = regexPattern.Substring(3);
                regexPattern = @"(^\b)*?" + regexPattern;
            }

            regexPattern = @"\b" + regexPattern + @"\b";

            return regexPattern;
        }
    }

}
