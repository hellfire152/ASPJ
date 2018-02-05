using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class OTP
    {
        
        [Display(Name = "Enter OTP: ")]
        public string OTPvalue { get; set; }



    }
}