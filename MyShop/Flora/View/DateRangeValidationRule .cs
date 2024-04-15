using System;
using System.Globalization;
using System.Windows.Controls;

namespace Flora.View
{
    public class DateRangeValidationRule : ValidationRule
    {
        public class DateTimeRange
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTimeRange dateRange)
            {
                if (dateRange.StartDate > dateRange.EndDate)
                {
                    return new ValidationResult(false, "Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
                }

                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Giá trị không hợp lệ.");
        }
    }
}
