using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.Model
{
    public class Order
    {
        public int Number { get; set; }
        public string OrderID { get; set; }
        public string Customer { get; set; }
        public int Quantity { get; set; }
        public string CostTotal { get; set; }
        public string OrderedTime { get; set; }
        public string Status { get; set; }
    }
}
