using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.Context;
using System.IO;
using System.Net;

namespace ASPJ_Project.Controllers
{
    public class ForumController : Controller
    {
        private ForumContext db = new ForumContext();
        // GET: Forum
        //public ActionResult Index(string currentFilter, string searchString, int? page)
        //{
        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }
        //    ViewBag.CurrentView = searchString;
        //    return View();
        //}

        public ActionResult Home()
        {

            return View(db.threads.ToList());
        }
        [HttpGet]
        public ActionResult Thread()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateThread()
        {
            return View();
        }
        public ActionResult Home2()
        {
            return View(db.threads.ToList());
        }

        [HttpGet]
        public ActionResult GetThread(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Home");
            }
            //ThreadId = RouteData.Values["ThreadId"].ToString();
            //ForumHome threaddata = new ForumHome();
            Thread thread = db.threads.Find(id);
            if (thread == null)
                return HttpNotFound();
            return View(thread);
        }

        [HttpPost]
        public ActionResult CreateThread(Thread thread)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(thread.image != null)
                    {
                        HttpPostedFileBase UploadedImage = thread.image;
                        var UploadedImageFileName = Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
                        string ext = Path.GetExtension(UploadedImage.FileName);
                        bool isValidFile = false;
                        if (UploadedImage.ContentLength > 0)
                        {
                            if (ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg")
                            {
                                isValidFile = true;
                            }
                            if (!isValidFile)
                            {
                                ViewBag.Message = "Invalid File. Please upload an image file ";
                            }
                            else
                            {
                                int fileSize = UploadedImage.ContentLength;
                                if (fileSize > 2097152)
                                {
                                    ViewBag.Message = "Maximum file size (2MB) exceeded";
                                }
                                else
                                {
                                    string ImageFileName = Path.GetFileName(UploadedImageFileName) + Path.GetExtension(UploadedImage.FileName);
                                    string FolderPath = Path.Combine(Server.MapPath("~/Content/UploadedImages"), ImageFileName);
                                
                                    UploadedImage.SaveAs(FolderPath);
                                    ViewBag.Message = "File uploaded successfully.";
                                    thread.imageName = ImageFileName;

                                }

                            }
                        }
                    }

                    thread.votes = 0;
                    thread.date = DateTime.Now;
                    db.threads.Add(thread);
                    db.SaveChanges();
                    return RedirectToAction("Home");


                }
                return View(thread);
            }
            catch
            {
                return View();
            }

        }
        //Get: /Forum/DeleteThread/
        [HttpGet]
        public ActionResult DeleteThread(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Thread thread = db.threads.Find(id);
            if (thread == null)
                return HttpNotFound();
            return View(thread);
        }

        //Post: /Forum/DeleteThread/
        [HttpPost, ActionName("DeleteThread")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteThreadConfirmed(int? id)
        {
            try
            {
                Thread thread = new Thread();
                    if (ModelState.IsValid)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    thread = db.threads.Find(id);
                    if (thread == null)
                        return HttpNotFound();
                    db.threads.Remove(thread);
                    db.SaveChanges();
                    return RedirectToAction("Home");
                }
                    return View(thread);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult MyThreads()
        {
            return View(db.threads.ToList());
        }
        [HttpGet]
        public ActionResult MyComments()
        {
            return View(db.comments.ToList());
        }
        [HttpPost, ActionName("MyComments")]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(int? id, Comment comment)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Thread thread = db.threads.Find(id);
                    if (thread == null)
                        return HttpNotFound();

                    comment.threadId = thread.id;
                    comment.username = "Barry Allen";
                    comment.date = DateTime.Now;
                    db.comments.Add(comment);
                    db.SaveChanges();

                }
                

                return GetThread(id);

            }
            catch
            {
                return GetThread(id);
            }
        }

    }
}