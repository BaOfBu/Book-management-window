using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Flora.Model;
using Flora.View;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace Flora.ViewModel
{
    class OrderVM : Utilities.ViewModelBase
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
        private BindingList<Order> _orderList;
        public BindingList<Order> OrderList
        {
            get { return _orderList; }
            set
            {
                if(_orderList != value) { 
                    _orderList = value;
                    OnPropertyChanged("OrderList");
                }
            }
        }

        public OrderVM() {
            OrderList = new BindingList<Order>()
            {
                new Order { Number = 1, OrderID = "100345489", Customer = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 2, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 3, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 4, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivered"},
                new Order { Number = 5, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 6, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 7, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 8, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 9, OrderID = "100345489", Customer = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 10, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 11, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 12, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivered"},
                new Order { Number = 13, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 14, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 15, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
                new Order { Number = 16, OrderID = "100345489", Customer = "tuyetkydieu", Quantity = 1, CostTotal = "100000", OrderedTime = "03/05/2024", Status = "Delivering"},
            };
            PageSize = 8;
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
        }
    }
}
