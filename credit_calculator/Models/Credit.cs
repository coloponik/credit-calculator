using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using credit_calculator.Annotations;

namespace credit_calculator.Models
{
    public class Credit
    {
        [DisplayName("Credit amount:")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Must be positive number (limit to 2 decimal places)")]
        [Range(20000.00, 250000.00, ErrorMessage = "Must be between 20 000 and 250 000")]
        [NotValidSum(ErrorMessage = "Range: 20 000 - 250 000 (only 2 decimal places)")]
        public double CreditSum { get; set; }

        [DisplayName("Credit term (in months):")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+", ErrorMessage = "Must be positive integer number")]
        [Range(1, 36, ErrorMessage = "Must be between 1 and 36")]
        [NotValidTerm(ErrorMessage = "Must be integer number between 1 and 36")]
        public double CreditPeriod { get; set; }

        [DisplayName("Interest rate (per year):")]
        [Required(ErrorMessage = "Field is required")]
        [RegularExpression(@"\d+([.]\d{1,2})?", ErrorMessage = "Must be positive number (limit to 2 decimal places)")]
        [Range(10.00, 25.00, ErrorMessage ="Must be beetween 10 and 25")]
        [NotValidRate(ErrorMessage = "Must be positive beetween 10 and 25")]
        public double CreditRateYear { get; set; }
    }
}