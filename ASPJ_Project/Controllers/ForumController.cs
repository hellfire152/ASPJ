using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPJ_Project.Models;
using ASPJ_Project.ViewModels;

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

        public ActionResult Thread()
        {
            return View();
        }

    }
}