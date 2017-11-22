using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ASPJ_Project.Models
{
    public enum Modulator
    {
        True, False
    }
    public class ForumUser
    {
        public int UserID { get; set; }
        public int Username { get; set; }
        public Modulator? Modulator { get; set; }
        public virtual ICollection<Thread> Threads { get; set; }
    }
}