using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.Model
{
    public partial class HomeProductPreview
    {
        public int ProductIndex { get; set; }
        public int PlantId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string CategoryName { get; set; }
    }
}