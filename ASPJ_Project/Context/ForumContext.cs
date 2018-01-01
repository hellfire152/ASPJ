using ASPJ_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Context
{
    public class ForumContext: DbContext
    {
        public DbSet<Thread> threads { get; set; }
        public DbSet<Comment> comments { get; set; }
    }
}