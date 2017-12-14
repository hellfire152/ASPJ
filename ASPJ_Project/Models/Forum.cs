using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace ASPJ_Project.Models
{
    public enum Modulator
    {
        True, False
    }
    public class Forum
    {
     
    }

    public class ForumUser
    {
        public int UserId { get; set; }
        public int Username { get; set; }
        public Modulator? Modulator { get; set; }
        public ICollection<Thread> Threads { get; set; }
    }

    public class Comment
    {
        [Key]
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int ThreadId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]

        public DateTime Date { get; set; }
    }
    public class Thread
    {
        [Key]
        [Required]
        public int ThreadId { get; set; }
        [Required(AllowEmptyStrings=false)]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public int Votes { get; set; }
        [Required]
        public string Username { get; set; }
        //public virtual ForumUser ForumUser { get; set; }
    }
}