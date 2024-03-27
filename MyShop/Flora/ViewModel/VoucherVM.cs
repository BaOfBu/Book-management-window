using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class VoucherVM : Utilities.ViewModelBase
    {
        public List<string> PagesNumberList { get; set; }
        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged("PageSize");
                }
            }
        }
        private BindingList<Coupon> _voucherList;
        public BindingList<Coupon> VoucherList
        {
            get { return _voucherList; }
            set
            {
                if (_voucherList != value)
                {
                    _voucherList = value;
                    OnPropertyChanged("VoucherList");
                }
            }
        }

        public VoucherVM()
        {
            /*
            VoucherList = new BindingList<Voucher>()
            {
                new Voucher { Number = 1, VoucherID = "100345489", VoucherName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 2, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 3, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 4, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Active"},
                new Voucher { Number = 5, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 6, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 7, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 8, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 9, VoucherID = "100345489", VoucherName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 10, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 11, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 12, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Active"},
                new Voucher { Number = 13, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 14, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 15, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
                new Voucher { Number = 16, VoucherID = "100345489", VoucherName = "tuyetkydieu", StartDate = "03/01/2024", EndDate = "03/05/2024", Status = "Pending"},
            };
            */
            PageSize = 8;
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
        }
    }
}
