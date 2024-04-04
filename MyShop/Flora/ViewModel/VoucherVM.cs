using Flora.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public string SearchText { get; set; }
        public DateOnly? SelectedStartDate { get; set; }

        public DateOnly? SelectedEndDate { get; set; }
        public System.Windows.Input.ICommand LoadDataChangedCommand { get; set; }
        public System.Windows.Input.ICommand PageSizeChangedCommand { get; set; }
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
            PageNumber = 1;
            CouponList = new ObservableCollection<Coupon>();
            LoadDataForCurrentPage(PageNumber);

            SelectedStartDate = default;
            SelectedEndDate = default;

            LoadDataChangedCommand = new RelayCommand(LoadDataForCurrentPage);
            PageSizeChangedCommand = new RelayCommand(PageSizeChanged);
            SearchVoucherCommand = new RelayCommand(SearchHandle);
            FilterVoucherCommand = new RelayCommand(FilterByRangeDate);
            ReloadVoucherCommand = new RelayCommand(ReloadCoupons);
            RemoveVoucherCommand = new RelayCommand(RemoveCoupon);
            UpdateVoucherCommand = new RelayCommand(UpdateCoupon);
            StartDateChangedCommand = new RelayCommand(StartDateChanged);
            EndDateChangedCommand = new RelayCommand(EndDateChanged);
        }
        private ObservableCollection<Coupon> GetCouponsFromDatabase(int startIndex, int endIndex)
        {
            IQueryable<Coupon> query = _shopContext.Coupons;

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(o => o.CouponId.ToString().Contains(SearchText) || o.CouponCode.Contains(SearchText));
            }

            if (SelectedStartDate != null && SelectedEndDate != null)
            {
                query = query.Where(o => (o.StartDate >= SelectedStartDate) && (o.ExpiryDate < SelectedEndDate));
            }

            TotalItems = query.Count();

            var coupons = query
                        .Skip(startIndex)
                        .Take(endIndex)
                        .ToList();

            return new ObservableCollection<Coupon>(coupons);
        }

        private void LoadDataForCurrentPage(object page)
        {
            int pageNumber = Int32.Parse(page.ToString());

            int skip = (pageNumber - 1) * PageSize;
            try
            {
                CouponList.Clear();
                CouponList = GetCouponsFromDatabase(skip, PageSize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        private void PageSizeChanged(object pageSize)
        {
            PageSize = (int)pageSize;
            LoadDataForCurrentPage(PageNumber);
        }
        private void SearchHandle(object obj)
        {
            var text = obj as string;
            if (text != null)
            {
                SearchText = text;
                LoadDataForCurrentPage(PageNumber);
            }
        }
        private void FilterByRangeDate(object obj)
        {
            LoadDataForCurrentPage(PageNumber);
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
            LoadDataForCurrentPage(PageNumber);
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
