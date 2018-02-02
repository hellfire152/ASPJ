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
     
        public string confirmPassword { get; set; }
        public string captcha { get; set; }
        public string phoneNumber { get; set; }
    }


    public class UserMetadata
    {
        [Display(Name = "First Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string lastName { get; set; }

        [Display(Name = "Email")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Username")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string userName { get; set; }

        //[Display(Name ="Date of birth")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:MM/dd/yyyy}")]
        //public DateTime DateOfBirth { get; set; }

        [Display(Name = "Password")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        //[MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password again")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confirm password and password do not match")]
        public string confirmPassword { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string phoneNumber { get; set; }

       // [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter this field")]
        public string captcha { get; set; }
    }

   
}