using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using MySql.Data.MySqlClient;

namespace ASPJ_Project.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }
        //public ActionResult ViewUser(DummyProfile dummy)
        //{
        //    DummyProfile user = dummy;
        //    ViewData["role"] = user.role;
        //    if (dummy.role == "Admin")
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToRoute("action","index");
        //    }
        //}

        public ActionResult Search()
        {
            //Session["role"] = "Admin";
            //string role = Session["role"].ToString();
            //if(role != "Admin")
            //{
            //    return RedirectToAction("Index", "Unauthorized");
            //}

            Database d = Database.CurrentInstance;
            Debug.WriteLine(d);
            List<DummyProfile> Dummys = new List<DummyProfile>();

            try
            {
              if (d.OpenConnection())
              {
                    string SearchQuery = "SELECT * FROM dummyuser";

                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            DummyProfile dummy = new DummyProfile
                            {
                                Id = (int.Parse(r["UserId"].ToString())),
                                Username = (r["UserName"].ToString()),
                                FirstName = (r["FirstName"]).ToString(),
                                Role = (r["Role"].ToString()),
                                Email = (r["Email"].ToString())
                            };
                            Dummys.Add(dummy);
                            ViewBag.Dummys = Dummys;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchModel model)
        {
            #region useless
            //if (search != null)
            //{
            //    List<Player> searchlist = new List<Player>();
            //    foreach(Player i in DB){
            //        if(i.username == search)
            //        {
            //            searchlist.Add(i);
            //        }
            //    }

            //    ViewData["match"] = searchlist;
            //}


            //code from video
            //return View(db.Users.Where(x => x.Name.StartsWith(search) || search == null).toList());
            #endregion

            #region db
            Database d = Database.CurrentInstance;

            List<DummyProfile> Dummys = new List<DummyProfile>();

            
            try
            {
                if (d.OpenConnection())
                {
                    string SearchQuery = "SELECT * FROM dummyuser WHERE username LIKE" + model.Search + ";";
                    
                    MySqlCommand c = new MySqlCommand(SearchQuery, d.conn);

                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                                DummyProfile dummy = new DummyProfile
                                {
                                    Id = (int.Parse(r["Id"].ToString())),
                                    Username = (r["username"].ToString()),
                                    Role = (r["role"].ToString()),
                                    Email = (r["email"].ToString()),
                                    Status = (r["status"].ToString())
                                };
                                Dummys.Add(dummy);
                                ViewBag.Dummys = Dummys;
                            
                        }
                    }
                }
            }catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }
            return View();
            #endregion

        }
    }
}
