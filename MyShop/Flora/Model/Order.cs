using System;
using System.Collections.Generic;

namespace Flora;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateOnly? OrderDate { get; set; }

    public int? CouponId { get; set; }

    public string Status { get; set; }

    public virtual Coupon Coupon { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public object Clone()
    {
        return MemberwiseClone();
    }
}
