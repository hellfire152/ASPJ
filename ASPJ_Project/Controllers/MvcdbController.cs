﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using System.Data.Entity;

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
    }
}
