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
    public class MoneyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidMoney(text);

            if (text.Length > 0)
            {
                if (reg == false)
                {
                    return new ValidationResult(false, $"Please enter a valid money.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidMoney(string money)
        {
            string emailPattern = "(-?[0-9]+[\\.]+[0-9]*)";
            Regex regex = new Regex(emailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(money);
        }
    }
}
