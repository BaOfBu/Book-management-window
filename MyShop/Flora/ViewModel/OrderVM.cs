using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Flora.Model;
using Flora.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Flora.ViewModel
{
    class OrderVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        private string _searchText;
        public ObservableCollection<Order> OrderList { get; set; }
        public ObservableCollection<string> PagesNumberList { get; set; }
        public int PageSize { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    SearchHandle();
                }
            }
        }

        private DateOnly? _selectedStartDate;
        public DateOnly? SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                    SearchHandle();
                }
            }
        }

        private DateOnly? _selectedEndDate;
        public DateOnly? SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                    SearchHandle();
                }
            }
        }

        public System.Windows.Input.ICommand RemoveOrderCommand { get; set; }
        public OrderVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new ObservableCollection<string> { "8", "16", "24", "32", "64", "96" };
            PageSize = 8;
            OrderList = new ObservableCollection<Order>();
            LoadOrders("");

            RemoveOrderCommand = new RelayCommand(RemoveOrder);
        }

        private List<Order> GetOrdersFromDatabase(string keyword)
        {
            var query = _shopContext.Orders
                        .Include(o => o.Coupon)
                        .Include(o => o.Customer)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Plant)
                        .Where(o => o.OrderId.ToString().Contains(keyword) || o.Customer.Name.Contains(keyword));

            if (_selectedStartDate != null && _selectedEndDate != null)
            {
                query = query.Where(o => o.OrderDate >= _selectedStartDate && o.OrderDate <= _selectedEndDate);
            }

            var result = query.ToList();
            return result;
        }

        private void LoadOrders(string keyword)
        {
            var orders = GetOrdersFromDatabase(keyword);
            OrderList = new ObservableCollection<Order>(orders);
        }

        private void SearchHandle()
        {
            LoadOrders(SearchText);
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

                if (OrderList != null)
                {
                    OrderList.Remove(selectedItem);
                }
            }
        }
    }
}
