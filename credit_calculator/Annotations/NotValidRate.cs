using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace credit_calculator.Annotations 
{

    public class NotValidRate : ValidationAttribute 
    {

        public override bool IsValid(object value) 
        {

            if (value != null) 
            {

                Regex regex = new Regex(@"\d+([.]\d{1,2})?");
                string str = value.ToString();

                if (regex.IsMatch(str)) 
                {
                    double doubval = Convert.ToDouble(value);
                    if ((doubval >= 0.50) && (doubval <= 25.00)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}