using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Flora.Model;
using Microsoft.EntityFrameworkCore;

namespace Flora.ViewModel
{
    class OrderVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        private string _searchText;
        private PreviewOrder _orderSelected;

        public ObservableCollection<PreviewOrder> OrderList { get; set; }
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

        public PreviewOrder OrderSelected
        {
            get => _orderSelected;
            set
            {
                if (_orderSelected != value)
                {
                    _orderSelected = value;
                    OnPropertyChanged(nameof(OrderSelected));
                    RemoveOrder();
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

        public OrderVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new ObservableCollection<string> { "8", "16", "24", "32", "64", "96" };
            PageSize = 8;
            OrderList = new ObservableCollection<PreviewOrder>();
            LoadOrders("");
        }

        private List<Order> GetOrdersFromDatabase(string keyword)
        {
            var query = _shopContext.Orders
                        .Include(o => o.Customer)
                        .Where(o => o.OrderId.ToString().Contains(keyword) || o.Customer.Name.Contains(keyword));

            if (_selectedStartDate != null && _selectedEndDate != null)
            {
                query = query.Where(o => o.OrderDate >= _selectedStartDate && o.OrderDate <= _selectedEndDate);
            }

            return query.ToList();
        }

        private ObservableCollection<PreviewOrder> CreatePreviewOrdersList(List<Order> orders)
        {
            var previewOrders = new ObservableCollection<PreviewOrder>();
            int orderIndex = 1;
            foreach (var o in orders)
            {
                previewOrders.Add(new PreviewOrder
                {
                    OrderIndex = orderIndex++,
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.Name,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                });
            }
            return previewOrders;
        }

        private void LoadOrders(string keyword)
        {
            var orders = GetOrdersFromDatabase(keyword);
            OrderList = CreatePreviewOrdersList(orders);
        }

        private void SearchHandle()
        {
            LoadOrders(SearchText);
        }

        private void RemoveOrder()
        {
            var orderToRemove = _shopContext.Orders.SingleOrDefault(o => _orderSelected != null && o.OrderId == _orderSelected.OrderId);

            if (orderToRemove != null)
            {
                var orderDetails = _shopContext.OrderDetails.Where(o => o.OrderId == orderToRemove.OrderId).ToList();
                _shopContext.OrderDetails.RemoveRange(orderDetails);

                _shopContext.Orders.Remove(orderToRemove);
                _shopContext.SaveChanges();

                if (OrderList != null)
                {
                    OrderList.Remove(_orderSelected);
                }
            }
        }
    }
}
