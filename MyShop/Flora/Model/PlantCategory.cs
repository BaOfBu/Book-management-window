﻿using System.Collections.Generic;

namespace Flora;

public partial class PlantCategory
{
    public string Image { get; set; }
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Plant> Plants { get; set; } = new List<Plant>();
}