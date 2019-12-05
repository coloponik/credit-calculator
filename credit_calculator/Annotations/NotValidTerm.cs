using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace credit_calculator.Annotations
{
    public class NotValidTerm : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null) 
            {
                Regex regex = new Regex(@"\d+");
                string str = value.ToString();
                if (regex.IsMatch(str)) 
                {
                    double doubval = Convert.ToDouble(value);
                    if ((doubval >= 1) && (doubval <= 60)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}