using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using MySql.Data.MySqlClient;

namespace RBACDemo.Areas.Steria.Controllers
{
    public class UnauthorisedController : Controller
    {
        // GET: Unauthorised
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Banned()
        {
            string id ="";
            if(TempData["banID"] != null ||TempData["banID"].ToString() != "")
            {
                id = TempData["banID"].ToString();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
             
            
            Database d = Database.CurrentInstance;
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dububase.banhistory where banid = @banid";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);
                    c.Parameters.AddWithValue("@banid", Convert.ToInt32(id));
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            TempData["banPeriod"] = r["banPeriod"].ToString();
                            TempData["banReason"] = r["banReason"].ToString();
                            return View();
                        }
                    }
                }
            }
            catch (MySqlException e)
            {

                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }


            return View();
        }
    }
}