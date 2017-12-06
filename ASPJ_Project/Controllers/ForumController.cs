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
            var threads = new List<Thread>
            {
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 },
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 },
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 }
            };

            var viewModel = new ForumViewModel
            {
                Threads = threads
            };
            return View(viewModel);
        }
        public ActionResult Home2()
        {
            var threads = new List<Thread>
            {
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 },
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 },
                new Thread{ Title = "Flash is missing", Username = "Alex Ang", Date = DateTime.Now, Votes = 0 }
            };
            return View(threads);
        }
        public ActionResult CreateThread()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Thread()
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

        [HttpPost]
        public void UploadImage_Click(Thread thread)
        {
            HttpPostedFileBase UploadedImage = thread.Image;
            string ext = System.IO.Path.GetExtension(UploadedImage.FileName);
            bool isValidFile = false;
            ViewBag.message = "hi the type is " + ext;
            if (UploadedImage.ContentLength > 0)
            {
                if (ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpeg")
                {
                    isValidFile = true;
                }
                if (!isValidFile)
                {
                    ViewBag.Message.ForeColor = System.Drawing.Color.Red;
                    ViewBag.Message.Text = "Invalid File. Please upload a File with extension ";
                }
                else
                {
                    int fileSize = UploadedImage.ContentLength;
                    if (fileSize > 2097152)
                    {
                        ViewBag.Message.ForeColor = System.Drawing.Color.Red;
                        ViewBag.Message = "Maximum file size (2MB) exceeded";
                    }
                    else
                    {
                        ViewBag.Message.ForeColor = System.Drawing.Color.Green;
                        ViewBag.Message = "File uploaded successfully.";
                    }

                }
            }
            else
            {
                ViewBag.Message.ForeColor = System.Drawing.Color.Red;
                ViewBag.Message = "Please select a file to upload";
            }

        }

    }
}