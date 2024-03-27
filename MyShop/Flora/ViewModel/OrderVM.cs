using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Flora.View;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using Microsoft.EntityFrameworkCore;
using Flora.Model;

namespace Flora.ViewModel
{
    class OrderVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        private int _pageSize;
        private string _searchText;
        private BindingList<PreviewOrder> _orderList;

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

        public BindingList<PreviewOrder> OrderList
        {
            get { return _orderList; }
            set
            {
                if (_orderList != value)
                {
                    _orderList = value;
                    OnPropertyChanged("OrderList");
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

        public OrderVM()
        {
            _shopContext = new MyShopContext();
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            PageSize = 8;
            LoadOrders("");
        }

        private void LoadOrders(string keyword)
        {
            var orders = _shopContext.Orders
                .Include(o => o.Customer)
                .Where(o => o.OrderId.ToString().Contains(keyword) || o.Customer.Name.Contains(keyword))
                .ToList();

            int orderIndex = 1;

            OrderList = new BindingList<PreviewOrder>(
                orders.Select(o => new PreviewOrder
                {
                    OrderIndex = orderIndex++,
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.Name,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                }).ToList());
        }

        private void SearchHandle()
        {
            LoadOrders(SearchText);
        }
    }
}
