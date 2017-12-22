using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;

namespace ASPJ_Project.Controllers
{
    public class MvcdbController : Controller
    {
        // GET: Mvcdb
        public ActionResult Index()
        {
            List<user> userList = new List<user>();
            using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                userList = dbModel.users.ToList<user>();
            }
            return View(userList);
        }

        // GET: Mvcdb/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            using(mvccruddbEntities dbModel = new mvccruddbEntities())
            {
                dbModel.users.Add(userModel);
                dbModel.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Mvcdb/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mvcdb/Edit/5
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

        // GET: Mvcdb/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mvcdb/Delete/5
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
