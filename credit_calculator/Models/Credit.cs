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
        [Range(1000, 3000000, ErrorMessage = "Invalid value")]
        public double CreditSum { get; set; }

        [DisplayName("Credit term:")]
        [Required(ErrorMessage = "Field is required")]
        [Range(1, 1825, ErrorMessage = "Invalid value")]
        public double CreditPeriod { get; set; }

        [DisplayName("Interest rate:")]
        [Required(ErrorMessage = "field is required")]
        [Range(1, 25, ErrorMessage ="Invalid value")]
        public double CreditRateYear { get; set; }
    }
}