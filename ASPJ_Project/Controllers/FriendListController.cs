using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;

namespace ASPJ_Project.Controllers
{
    public class FriendListController : Controller
    {
        // GET: FriendList
        public ActionResult Index()
        {
            List<userdetail> userList = new List<userdetail>();
            using (friendlistdbEntities dbModel = new friendlistdbEntities())
            {
                userList = dbModel.userdetails.ToList<userdetail>();
            }
            return View(userList);
        }

        // GET: FriendList/Details/5
        public ActionResult Details(int id)
        {
            userdetail userModel = new userdetail();
            using(friendlistdbEntities dbModel = new friendlistdbEntities())
            {
                userModel = dbModel.userdetails.Where(x => x.UserID == id).FirstOrDefault();
            }
            return View(userModel);
        }

        // GET: FriendList/Create
        public ActionResult Create()
        {
            return View(new userdetail());
        }

        // POST: FriendList/Create
        [HttpPost]
        public ActionResult Create(userdetail userModel)
        {
             
          using(friendlistdbEntities dbModel = new friendlistdbEntities())
            {
                dbModel.userdetails.Add(userModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: FriendList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FriendList/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FriendList/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
