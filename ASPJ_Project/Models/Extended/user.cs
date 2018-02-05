using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ASPJ_Project.Models;

namespace ASPJ_Project.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class user
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string confirmPassword { get; set; }
        public string captcha { get; set; }
        public string phoneNumber { get; set; }
    }


    public class UserMetadata
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string lastName { get; set; }

        [System.Web.Mvc.Remote("IsEmailExists", "User", ErrorMessage = "Email already in use")]
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [System.Web.Mvc.Remote("IsUsernameExists", "User",ErrorMessage="User Name already in use")]  
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string userName { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet and 1 Number.")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password again")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confirm password and password do not match")]
        public string confirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?:\d{8}|00\d{10}|\+\d{2}\d{8})$", ErrorMessage = "Please input your country code, followed by phone number. (+xx xxxx xxxx)")]
        public string phoneNumber { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter this field")]
        public string captcha { get; set; }
    }

   
}