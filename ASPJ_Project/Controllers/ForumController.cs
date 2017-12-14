using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.ViewModels;
using System.IO;

namespace ASPJ_Project.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentView = searchString;
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
    
        public ActionResult CreateThread()
        {
            var newThread = new Thread();
            return View();
        }
        [HttpGet]
        public ActionResult Thread()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetThread(int id)
        {
            if (RouteData.Values["id"] == null)
            {
                return Home();
            }
            //ThreadId = RouteData.Values["ThreadId"].ToString();
            ForumHome threaddata = new ForumHome();
            var thread = threaddata.getThreadData().ToList().Find(p => p.Key == id);
            ViewBag.Thread = thread;
            return View();
        }
        public ActionResult Home2()
        {
            return View();
        }
	
        [HttpPost]
        public ActionResult CreateThread(Thread thread)
        {
            HttpPostedFileBase UploadedImage = thread.Image;
            string ext = Path.GetExtension(UploadedImage.FileName);
            bool isValidFile = false;
            if (UploadedImage.ContentLength > 0)
            {
                if (ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpeg")
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
                        string ImageFileName = Path.GetFileName(UploadedImage.FileName);

                        string FolderPath = Path.Combine(Server.MapPath("~/Content/UploadedImages"), ImageFileName);

                        UploadedImage.SaveAs(FolderPath);
                        ViewBag.Message = "File uploaded successfully.";
                    }

                }
            }
            return View();
        }


    }
}