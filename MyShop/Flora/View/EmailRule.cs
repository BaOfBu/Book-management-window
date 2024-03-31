using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Flora.View
{
    public class EmailRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidEmail(text);

            if (text.Length > 0 && text.Length <= 100)
            {
                if (reg == false)
                {
                    return new ValidationResult(false, $"Please enter a valid email.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidEmail(string email)
        {
            string emailPattern = "^([a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,6})*$";
            Regex regex = new Regex(emailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

    }
}
