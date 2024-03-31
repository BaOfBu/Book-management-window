using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using Flora.Utilities;
using Microsoft.EntityFrameworkCore;
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
        public int TotalItems { get; set; }

        public string SearchText { get; set; }

        public DateOnly? SelectedStartDate { get; set; }

        public DateOnly? SelectedEndDate { get; set; }
        public System.Windows.Input.ICommand SearchOrderCommand {  get; set; }
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
            OrderList = new ObservableCollection<Order>();
            TotalItems = 32;
            LoadOrders();

            SelectedStartDate = default; 
            SelectedEndDate = default;

            SearchOrderCommand = new RelayCommand(SearchHandle);
            FilterOrderCommand = new RelayCommand(FilterByRangeDate);
            RemoveOrderCommand = new RelayCommand(RemoveOrder);
            ReloadOrderCommand = new RelayCommand(ReloadOrders);
            StartDateChangedCommand = new RelayCommand(StartDateChanged);
            EndDateChangedCommand = new RelayCommand(EndDateChanged);
        }

        private List<Order> GetOrdersFromDatabase()
        {
            var query = _shopContext.Orders
                        .Include(o => o.Coupon)
                        .Include(o => o.Customer)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Plant)
                        .Where(o => o.OrderId.ToString().Contains(SearchText) || o.Customer.Name.Contains(SearchText));


            if (SelectedStartDate != null && SelectedEndDate != null)
            {
                query = query.Where(o => (o.OrderDate >= SelectedStartDate) && (o.OrderDate < SelectedEndDate));
            }
            
            return query.ToList();
        }

        private void LoadOrders()
        {
            var orders = GetOrdersFromDatabase();
            OrderList = new ObservableCollection<Order>(orders);
        }
        private void SearchHandle(object obj)
        {
            var text = obj as string;
            if (text != null)
            {
                SearchText = text;
                LoadOrders();
            }
        }
        private void FilterByRangeDate(object obj)
        {
            LoadOrders();
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
            LoadOrders();
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
