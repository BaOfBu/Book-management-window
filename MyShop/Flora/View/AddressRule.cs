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
    public class AddressRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidAddress(text);
            if (reg == false)
            {
                return new ValidationResult(false, $"Please enter a valid address. (up to 100 characters)");
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidAddress(string address)
        {
            if (address.Length < 0 || address.Length > 100)
            {
                return false;
            }
            return true;
        }
    }
}
