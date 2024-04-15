using System;
using System.Collections.Generic;
using static Azure.Core.HttpHeader;

namespace Flora;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? CouponId { get; set; }

    public string Status { get; set; }

    public virtual Coupon Coupon { get; set; }

    public virtual Customer Customer { get; set; }
}