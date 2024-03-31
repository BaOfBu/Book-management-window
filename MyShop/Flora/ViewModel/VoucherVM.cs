using Flora.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class VoucherVM : Utilities.ViewModelBase
    {
        private readonly MyShopContext _shopContext;
        public ObservableCollection<Coupon> CouponList { get; set; }
        public ObservableCollection<string> PagesNumberList { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public DateOnly? SelectedStartDate { get; set; }

        public DateOnly? SelectedEndDate { get; set; }
        public System.Windows.Input.ICommand SearchVoucherCommand { get; set; }
        public System.Windows.Input.ICommand FilterVoucherCommand { get; set; }
        public System.Windows.Input.ICommand ReloadVoucherCommand { get; set; }
        public System.Windows.Input.ICommand RemoveVoucherCommand { get; set; }
        public System.Windows.Input.ICommand UpdateVoucherCommand { get; set; }
        public System.Windows.Input.ICommand StartDateChangedCommand { get; set; }
        public System.Windows.Input.ICommand EndDateChangedCommand { get; set; }
        public VoucherVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new ObservableCollection<string> { "8", "16", "24", "32", "64", "96" };
            SearchText = string.Empty;
            PageSize = 8;
            CouponList = new ObservableCollection<Coupon>();
            LoadCoupons();

            SelectedStartDate = default;
            SelectedEndDate = default;

            SearchVoucherCommand = new RelayCommand(SearchHandle);
            FilterVoucherCommand = new RelayCommand(FilterByRangeDate);
            ReloadVoucherCommand = new RelayCommand(ReloadCoupons);
            RemoveVoucherCommand = new RelayCommand(RemoveCoupon);
            UpdateVoucherCommand = new RelayCommand(UpdateCoupon);
            StartDateChangedCommand = new RelayCommand(StartDateChanged);
            EndDateChangedCommand = new RelayCommand(EndDateChanged);
        }

        private List<Coupon> GetCouponsFromDatabase()
        {

            var query = _shopContext.Coupons
                        .Where(o => o.CouponId.ToString().Contains(SearchText) || o.CouponCode.Contains(SearchText));

            if (SelectedStartDate != null && SelectedEndDate != null)
            {
                query = query.Where(o => (o.StartDate >= SelectedStartDate) && (o.ExpiryDate <= SelectedEndDate));
            }

            return query.ToList();
        }
        private void LoadCoupons()
        {
            var coupons = GetCouponsFromDatabase();

            CouponList = new ObservableCollection<Coupon>(coupons);
        }
        private void SearchHandle(object obj)
        {
            var text = obj as string;
            if (text != null)
            {
                SearchText = text;
                LoadCoupons();
            }
        }
        private void FilterByRangeDate(object obj)
        {
            LoadCoupons();
        }
        private void RemoveCoupon(object selectedCoupon)
        {
            var selectedItem = selectedCoupon as Coupon;
            if (selectedItem != null)
            {
                _shopContext.Coupons.Remove(selectedItem);
                _shopContext.SaveChanges();

                CouponList?.Remove(selectedItem);
            }
        }
        private void UpdateCoupon(object selectedCoupon)
        {
            var selectedItem = selectedCoupon as Coupon;
            if(selectedItem != null)
            {
                var existingCoupon = _shopContext.Coupons.FirstOrDefault(c => c.CouponId == selectedItem.CouponId);
                if(existingCoupon != null)
                {
                    existingCoupon.CouponCode = selectedItem.CouponCode;
                    existingCoupon.Status = selectedItem.Status;
                    _shopContext.SaveChanges();
                }
            }
        }
        private void ReloadCoupons(object obj)
        {
            SelectedStartDate = default;
            SelectedEndDate = default;
            LoadCoupons();
        }
        private void StartDateChanged(object startDate)
        {
            var date = (DateTime)startDate;
            SelectedStartDate = DateOnly.FromDateTime(date.Date);
        }
        private void EndDateChanged(object endDate)
        {
            var date = (DateTime)endDate;
            SelectedEndDate = DateOnly.FromDateTime(date.Date);
        }
    }
}
