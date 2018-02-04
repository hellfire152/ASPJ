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

        //[Required(AllowEmptyStrings = false)]
        [DisplayName("Content")]
        //[StringLength(500, MinimumLength = 30, ErrorMessage = "Content cannot be longer than 500 characters and less than 30 characters")]
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
        public Thread thread { get; set;  }

    }
    public class Thread
    {
        [Key]
        public int id { get; set; }

        [Required(AllowEmptyStrings=false, ErrorMessage = "Title required!")]
        [DisplayName("Title")]
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters")]
        public string title { get; set; }

        [DisplayName("Date")]
        public DateTime date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content required!")]
        [DisplayName("Content")]
        [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
        public string content { get; set; }

        [NotMapped]
        [DisplayName("Image")]
        public HttpPostedFileBase image { get; set; }

        public String imageName { get; set; }


        [DisplayName("Votes")]
        public long votes { get; set; }

        [DisplayName("Username")]
        public string username { get; set; }

        public long comments { get; set; }

        public Boolean upVoted { get; set; }
        public Boolean downVoted { get; set; }
        //public virtual ForumUser ForumUser { get; set; }
    }
    
    public class Vote
    {
        [Key]
        public int id { get; set;  }

        public Boolean upvote { get; set; }
        public Boolean downvote { get; set; }
        public int userId { get; set; }
        [Required]
        public int threadId { get; set; }
        [Required]
        public String username { get; set; }

    }

    public class FileScan
    {
        public bool secure { get; set; }
        public string message { get; set; }
    }
}