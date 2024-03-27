using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.Model
{
    public class PreviewOrder
    {
        public int OrderIndex { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateOnly? OrderDate { get; set; }
        public string Status { get; set; }
    }
}
