using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

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
        public int id { get; set; }
        public int username { get; set; }
        public Modulator? modulator { get; set; }
        public ICollection<Thread> threads { get; set; }
    }

    public class Comment
    {
        [Key]
        public int id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Content")]
        public string content { get; set; }

        [Required]
        [DisplayName("Username")]
        public string username { get; set; }

        [Required]
        [DisplayName("Date")]
        public DateTime date { get; set; }

        [Required]
        [ForeignKey("Thread")]
        public int threadId { get; set; }
        public virtual Thread Thread { get; set;  }

    }
    public class Thread
    {
        [Key]
        public int id { get; set; }

        [Required(AllowEmptyStrings=false, ErrorMessage = "Title required!")]
        [DisplayName("Title")]
        public string title { get; set; }

        [Required]
        [DisplayName("Date")]
        public DateTime date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content required!")]
        [DisplayName("Content")]
        public string content { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public HttpPostedFileBase image { get; set; }

        public String imageName { get; set; }

        [Required]
        [DisplayName("Votes")]
        public int votes { get; set; }

        [Required]
        [DisplayName("Username")]
        public string username { get; set; }

        public virtual List<Comment> comments { get; set; }
        //public virtual ForumUser ForumUser { get; set; }
    }
}