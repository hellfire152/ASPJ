using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Thread
    {
        public int ThreadID { get; set; }
        public string title { get; set; }
        public DateTime PostedDate { get; set; }
        public int votes { get; set; }
        public string username { get; set; }



        //public virtual ForumUser ForumUser { get; set; }
    }
}