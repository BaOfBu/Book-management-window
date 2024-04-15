using Flora.Model;
using Microsoft.EntityFrameworkCore;
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
        private MyShopContext _shopContext;
        private int _pageSize;
        private string _searchText;
        private BindingList<Coupon> _couponList;
        public List<string> PagesNumberList { get; set; }
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
        public BindingList<Coupon> CouponList
        {
            get { return _couponList; }
            set
            {
                if (_couponList != value)
                {
                    _couponList = value;
                    OnPropertyChanged("VoucherList");
                }
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("SearchText");
                    SearchHandle();
                }
            }
        }

        public VoucherVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            PageSize = 8;
            LoadCoupons("");
        }

        private void LoadCoupons(string keyword)
        {
            var query = _shopContext.Coupons
                        .Where(o => o.CouponId.ToString().Contains(keyword) || o.CouponCode.Contains(keyword));

            var coupons = query.ToList();

            CouponList = new BindingList<Coupon>(coupons);
        }

        private void SearchHandle()
        {
            LoadCoupons(SearchText);
        }
    }
}
