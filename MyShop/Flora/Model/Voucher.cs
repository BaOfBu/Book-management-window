using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.Model
{
    public class Voucher
    {
        public int Number { get; set; }
        public string VoucherID { get; set; }
        public string VoucherName { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
    }
}
