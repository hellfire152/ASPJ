using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASPJ_Project.Models
{
    public class CreditCard
    {
        public struct Address
        {
            [MaxLength(25, ErrorMessage = "City has a maximum of 25 characters.")]
            public string city { get; set; }

            [StringLength(2, ErrorMessage = "Country code must be 2 characters long.", MinimumLength = 2)]
            public string country_code { get; set; }

            [Required(ErrorMessage = "Address is required.")]
            [MaxLength(50, ErrorMessage = "Address Line has a maximum of 50 characters.")]
            public string line1 { get; set; }

            [MaxLength(50, ErrorMessage = "Address Line has a maximum of 50 characters.")]
            public string line2 { get; set; }

            [Required(ErrorMessage = "Postal Code is required.")]
            [MaxLength(10, ErrorMessage = "Postal Code has a maxmimum of 10 characters.")]
            public string postal_code { get; set; }

            [MaxLength(25, ErrorMessage = "State has a maximum of 25 characters.")]
            public string state { get; set; }
        }

        public Address billing_address { get; set; }

        //[CreditCardNo]
        [Required(ErrorMessage = "Credit Card Number is required.")]
        [StringLength(16, ErrorMessage = "Credit card must be 16 digits long.", MinimumLength = 16)]
        public string creditCardNo { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [StringLength(3, ErrorMessage = "CVV must only be 3 digits long.", MinimumLength = 3)]
        public string cvv2 { get; set; }

        [Required(ErrorMessage = "Credit card expiry month is required.")]
        [Range(1,12)]
        public int expire_month { get; set; }

        [Required(ErrorMessage = "Credit card expiry year is required.")]
        [Range(2018, 2030)]
        public int expire_year { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(20, ErrorMessage = "First name has a maximum of 20 characters.")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(20, ErrorMessage = "First name has a maximum of 20 characters.")]
        public string last_name { get; set; }   
    }

    public class CreditCardNoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string creditCardNumber = Convert.ToString(value);

                //Luhn algorithm
                int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                                .Reverse()
                                .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                                .Sum((e) => e / 10 + e % 10);

                //// If the final sum is divisible by 10, then the credit card number
                //   is valid. If it is not divisible by 10, the number is invalid.
                if (sumOfDigits % 10 == 0)
                {
                    return ValidationResult.Success;
                }
                else
                    return new ValidationResult("Invalid Credit Card Number entered.");
            }
            return new ValidationResult("Invalid Credit Card Number entered.");
        }
    }
}