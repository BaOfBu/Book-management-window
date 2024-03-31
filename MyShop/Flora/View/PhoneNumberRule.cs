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
    public class PhoneNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidPhoneNumber(text);

            if (text.Length > 0)
            {
                if (reg == false)
                {
                    return new ValidationResult(false, $"Please enter a valid phone number.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            string phoneNumberPattern = "^(0|84)(2(0[3-9]|1[0-6|8|9]|2[0-2|5-9]|3[2-9]|4[0-9]|5[1|2|4-9]|6[0-3|9]|7[0-7]|8[0-9]|9[0-4|6|7|9])|3[2-9]|5[5|6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])([0-9]{7})$";
            Regex regex = new Regex(phoneNumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(phone);
        }
    }
}
