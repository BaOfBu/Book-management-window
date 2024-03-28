using System;
using System.Collections.Generic;

namespace Flora;

public partial class Coupon
{
    public int CouponId { get; set; }

    public string CouponCode { get; set; }

    public decimal? Discount { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
