using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASPJ_Project.Models
{
    public class ResetPassword
    {
        [Display(Name ="New Password")]
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        
    }
}