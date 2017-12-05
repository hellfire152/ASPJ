using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Thread
    {
        public int ThreadID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public HttpPostedFileBase Image{ get; set; }
        public int Votes { get; set; }
        public string Username { get; set; }
        //public virtual ForumUser ForumUser { get; set; }
    }
}