using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Flora.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Telerik.Windows.Controls;

namespace Flora.ViewModel
{
    class OrderVM : Utilities.ViewModelBase
    {
        private readonly MyShopContext _shopContext;
        public ObservableCollection<Order> OrderList { get; set; }
        public IEnumerable<Order> Items { get; set; }
        public ObservableCollection<string> PagesNumberList { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public string SearchText { get; set; }
        public DateOnly? SelectedStartDate { get; set; }

        public DateOnly? SelectedEndDate { get; set; }
        public System.Windows.Input.ICommand LoadDataChangedCommand { get; set; }
        public System.Windows.Input.ICommand PageSizeChangedCommand { get; set; }
        public System.Windows.Input.ICommand SearchOrderCommand { get; set; }
        public System.Windows.Input.ICommand FilterOrderCommand { get; set; }
        public System.Windows.Input.ICommand ReloadOrderCommand { get; set; }
        public System.Windows.Input.ICommand RemoveOrderCommand { get; set; }
        public System.Windows.Input.ICommand StartDateChangedCommand { get; set; }
        public System.Windows.Input.ICommand EndDateChangedCommand { get; set; }
        public OrderVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new ObservableCollection<string> { "8", "16", "24", "32", "64", "96" };
            SearchText = string.Empty;
            PageSize = 8;
            PageNumber = 1;
            OrderList = new ObservableCollection<Order>();
            LoadDataForCurrentPage(PageNumber);
            SelectedStartDate = default;
            SelectedEndDate = default;

            LoadDataChangedCommand = new RelayCommand(LoadDataForCurrentPage);
            PageSizeChangedCommand = new RelayCommand(PageSizeChanged);
            SearchOrderCommand = new RelayCommand(SearchHandle);
            FilterOrderCommand = new RelayCommand(FilterByRangeDate);
            RemoveOrderCommand = new RelayCommand(RemoveOrder);
            ReloadOrderCommand = new RelayCommand(ReloadOrders);
            StartDateChangedCommand = new RelayCommand(StartDateChanged);
            EndDateChangedCommand = new RelayCommand(EndDateChanged);
        }

        private ObservableCollection<Order> GetOrdersFromDatabase(int startIndex, int endIndex)
        {
            IQueryable<Order> query = _shopContext.Orders;

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(o => o.OrderId.ToString().Contains(SearchText) || o.Customer.Name.Contains(SearchText));
            }

            if (SelectedStartDate != null && SelectedEndDate != null)
            {
                query = query.Where(o => (o.OrderDate >= SelectedStartDate) && (o.OrderDate < SelectedEndDate));
            }
            
            TotalItems = query.Count();

            var orders = query
                        .Include(o => o.Coupon)
                        .Include(o => o.Customer)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Plant)
                        .Skip(startIndex)
                        .Take(endIndex)
                        .ToList();

            return new ObservableCollection<Order>(orders);
        }

        private void LoadDataForCurrentPage(object page)
        {
            int pageNumber = Int32.Parse(page.ToString());

            int skip = (pageNumber - 1) * PageSize;
            try
            {
                OrderList.Clear();
                OrderList = GetOrdersFromDatabase(skip, PageSize);
            }catch(Exception ex)
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
        private void RemoveOrder(object selectedOrder)
        {
            var selectedItem = selectedOrder as Order;
            if (selectedItem != null)
            {
                var orderDetails = _shopContext.OrderDetails.Where(o => o.OrderId == selectedItem.OrderId).ToList();
                _shopContext.OrderDetails.RemoveRange(orderDetails);

                _shopContext.Orders.Remove(selectedItem);
                _shopContext.SaveChanges();

                OrderList?.Remove(selectedItem);
            }
        }
        private void ReloadOrders(object obj)
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