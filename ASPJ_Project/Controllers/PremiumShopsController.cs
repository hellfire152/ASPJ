using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;

namespace ASPJ_Project.Controllers
{
    public class PremiumShopsController : Controller
    {
        private ASPJ_ProjectContext db = new ASPJ_ProjectContext();

        // GET: PremiumShops
        public ActionResult Index()
        {
            return View(db.PremiumShops.ToList());
        }

        // GET: PremiumShops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiumShop premiumShop = db.PremiumShops.Find(id);
            if (premiumShop == null)
            {
                return HttpNotFound();
            }
            return View(premiumShop);
        }

        // GET: PremiumShops/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PremiumShops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "itemID,itemName,itemType,itemDescription,beansPrice")] PremiumShop premiumShop)
        {
            if (ModelState.IsValid)
            {
                db.PremiumShops.Add(premiumShop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(premiumShop);
        }

        // GET: PremiumShops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiumShop premiumShop = db.PremiumShops.Find(id);
            if (premiumShop == null)
            {
                return HttpNotFound();
            }
            return View(premiumShop);
        }

        // POST: PremiumShops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "itemID,itemName,itemType,itemDescription,beansPrice")] PremiumShop premiumShop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premiumShop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(premiumShop);
        }

        // GET: PremiumShops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiumShop premiumShop = db.PremiumShops.Find(id);
            if (premiumShop == null)
            {
                return HttpNotFound();
            }
            return View(premiumShop);
        }

        // POST: PremiumShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PremiumShop premiumShop = db.PremiumShops.Find(id);
            db.PremiumShops.Remove(premiumShop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
