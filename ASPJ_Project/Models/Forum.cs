using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [Required]
        public int id { get; set; }
        public int Username { get; set; }
        public Modulator? Modulator { get; set; }
        public ICollection<Thread> Threads { get; set; }
    }

    public class Comment
    {
        [Key]
        [Required]
        public int id { get; set; }
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
        public int id { get; set; }
        [Required(AllowEmptyStrings=false)]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
        public String ImageName { get; set; }
        [Required]
        public int Votes { get; set; }
        [Required]
        public string Username { get; set; }
        //public virtual ForumUser ForumUser { get; set; }
    }
}