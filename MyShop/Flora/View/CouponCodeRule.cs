using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Flora.View
{
    public class CouponCodeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (string)value;

            var reg = IsValidCouponCode(text);

            if (reg == false)
            {
                return new ValidationResult(false, $"Please enter a valid coupon code. (up to 20 characters)");
            }

            return ValidationResult.ValidResult;
        }

        public static bool IsValidCouponCode(string name)
        {
            if (name.Length < 0 || name.Length > 20)
            {
                return false;
            }
            return true;
        }
    }
}
