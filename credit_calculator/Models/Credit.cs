using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace credit_calculator.Models
{
    public class Credit
    {
        [DisplayName("Credit amount:")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+", ErrorMessage = "Must be positive number")]
        [Range(20000, 250000, ErrorMessage = "Must be between 20 000 and 250 000")]
        public double CreditSum { get; set; }

        [DisplayName("Credit term (in months):")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+", ErrorMessage = "Must be positive number")]
        [Range(1, 36, ErrorMessage = "Must be between 1 and 36")]
        public double CreditPeriod { get; set; }

        [DisplayName("Interest rate (per year):")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Must be positive number (limit to 2 decimal places)")]
        [Range(10, 25, ErrorMessage ="Must be beetween 10 and 25")]
        public double CreditRateYear { get; set; }
    }
}