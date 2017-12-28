using ASPJ_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASPJ_Project.ViewModels
{
    public class ForumViewModel
    {
        public List<Thread> threads;
        public List<Thread> popularThreads;

    }
    public class ForumHome
    {
        public Dictionary<int, Thread> threads { get; set; }
        public Dictionary<int, Thread> popularThreads { get; set;}
        public Dictionary<int, Thread> getThreadData()
        {
            threads = new Dictionary<int, Thread>();
            var thread1 = new Thread { Title = "Flash is missing", Username = "Alex Ang", Content = "The flash has gone missing for 5 years", Date = DateTime.Now, Votes = 0 };
            var thread2 = new Thread { Title = "Arrow is missing", Username = "Alex Ang", Content = "The Arrow has gone missing for 5 years", Date = DateTime.Now, Votes = 0 };
            threads.Add(1, thread1);
            threads.Add(2, thread2);
            return threads;
        }

        public Dictionary<int, Thread> getPopularThreadData()
        {
            popularThreads = new Dictionary<int, Thread>();
            var thread1 = new Thread { Title = "Flash is missing", Username = "Alex Ang", Content = "The flash has gone missing for 5 years", Date = DateTime.Now, Votes = 100 };
            var thread2 = new Thread { Title = "Arrow is missing", Username = "Alex Ang", Content = "The Arrow has gone missing for 5 years", Date = DateTime.Now, Votes = 120 };
            popularThreads.Add(1, thread1);
            popularThreads.Add(2, thread2);
            return popularThreads;
        }
     
    }
}