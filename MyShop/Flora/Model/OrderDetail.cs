using System;
using System.Collections.Generic;

namespace Flora;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int PlantId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Order Order { get; set; }

    public virtual Plant Plant { get; set; }
    public object Clone()
    {
        return MemberwiseClone();
    }
}
