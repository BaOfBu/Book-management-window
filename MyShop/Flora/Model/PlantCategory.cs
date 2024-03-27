using System;
using System.Collections.Generic;

namespace Flora;

public partial class PlantCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Plant> Plants { get; set; } = new List<Plant>();
}