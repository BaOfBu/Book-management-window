using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Flora.View
{
    public class FullNameRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidFullName(text);

            if (reg == false)
            {
                return new ValidationResult(false, $"Please enter a valid full name. (up to 100 characters)");
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidFullName(string name)
        {
            if (name.Length < 0 || name.Length > 100)
            {
                return false;
            }
            return true;
        }
    }
}
