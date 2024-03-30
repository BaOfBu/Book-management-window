using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Flora.Model;
using System.Threading.Tasks;
using Flora.View;
using Microsoft.EntityFrameworkCore;
using Telerik.Windows.Controls.FieldList;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Flora.ViewModel
{
    class HomeVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;

        public ObservableCollection<HomeProductPreview> Plants { get; set; }

        public class BarInfo
        {
            public string Category { get; set; }
            public double Value { get; set; }
        }
        public class PieInfo
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public ObservableCollection<PieInfo> PieValues { get; set; }
        public ObservableCollection<BarInfo> TotalOrders { get; set; }
        public ObservableCollection<BarInfo> DeliveredOrders { get; set; }
        public string totalCustomer { get; set; }
        public string totalCustomerBuyThisMonth { get; set; }
        public string totalCustomerText { get; set; }
        public string totalDelivering { get; set; }
        public string thisMonthRevenue { get; set; }
        public string thisMonthRevenueText { get; set; }
        public string lastMonthRevenue { get; set; }
        public string increaseText { get; set; }
        public string totalProduct { get; set; }
        public HomeVM()
        {
            _shopContext = new MyShopContext();
            Plants = get5LeastProduct();
            PieValues = getNumberOrdersOnStatus();
            TotalOrders = getInfo();
            DeliveredOrders = getInfoDelivered();

            totalCustomer = getTotalCustomer();
            totalCustomerBuyThisMonth = getTotalCustomerBuyThisMonth();
            totalCustomerText = $"{totalCustomerBuyThisMonth} customers have buy this month";

            totalDelivering = getTotalDelivering();
            thisMonthRevenue = getThisMonthRevenue();
            thisMonthRevenueText = $"${thisMonthRevenue}";
            lastMonthRevenue = getLastMonthRevenue();
            totalProduct = getTotalAvailableProduct();


            if(Convert.ToDouble(thisMonthRevenue) > Convert.ToDouble(lastMonthRevenue))
            {
                var increase = (Convert.ToDouble(thisMonthRevenue) - Convert.ToDouble(lastMonthRevenue)) / Convert.ToDouble(lastMonthRevenue) * 100;
                string increaseString = increase.ToString("#.00");
                increaseText = $"{increaseString}% Increase from last month";
            }
            else
            {
                var decrease = (Convert.ToDouble(lastMonthRevenue) - Convert.ToDouble(thisMonthRevenue)) / Convert.ToDouble(lastMonthRevenue) * 100;
                string decreaseString = decrease.ToString("#.00");
                increaseText = $"{decreaseString}% Decrease from last month";
            }

        }   

        private ObservableCollection<HomeProductPreview> get5LeastProduct()
        {
            var query = _shopContext.Plants
                        .OrderBy(p => p.StockQuantity)
            .Include(p => p.Category);
            var products = query.Take(5).ToList();
            int productIndex = 1;

            var plants = new ObservableCollection<HomeProductPreview>();
            foreach (var o in products)
            {
                plants.Add(new HomeProductPreview
                {
                    ProductIndex = productIndex++,
                    PlantId = o.PlantId,
                    Name = o.Name,
                    Quantity = o.StockQuantity,
                    Price = (decimal)o.Price,
                    CategoryName = o.Category.CategoryName
                });
            }
            return plants;
        }

        private ObservableCollection<PieInfo> getNumberOrdersOnStatus()
        {
            var query = from o in _shopContext.Orders
                        .GroupBy(o => o.Status)
                        .Select(g => new { Status = g.Key, Count = g.Count() })
                        select o;
            var orders = query.ToList();


            var values = new ObservableCollection<PieInfo>();

            values.Add(new PieInfo
            {
                Status = "Delivering",
                Count = 0
            });
            values.Add(new PieInfo
            {
                Status = "Delivered",
                Count = 0
            });
            values.Add(new PieInfo
            {
                Status = "Cancelled",
                Count = 0
            });
            foreach (var o in query)
            {
                if(o.Status == "Delivering")
                {
                    values[0].Count = o.Count;
                }
                else if (o.Status == "Delivered")
                {
                    values[1].Count = o.Count;
                }
                else if (o.Status == "Cancelled")
                {
                    values[2].Count = o.Count;
                }
            }
            return values;
        }
        private ObservableCollection<BarInfo> getInfo()
        {
            var ordersByMonth = _shopContext.Orders
                            .GroupBy(order => new { orderDate = order.OrderDate.Value.Month, orderDateYear = order.OrderDate.Value.Year })
                            .Select(group => new { Month = group.Key.orderDate, Year = group.Key.orderDateYear, Count = group.Count() })
                            .OrderBy(o => o.Year)
                            .ToList();
            var values = new ObservableCollection<BarInfo>();
            foreach (var o in ordersByMonth)
            {
                values.Add(new BarInfo
                {
                    Category = o.Month.ToString() + "/" + o.Year.ToString(),
                    Value = o.Count
                });
            }
            return values;
        }
        private ObservableCollection<BarInfo> getInfoDelivered()
        {
            var ordersByMonth = _shopContext.Orders
                            .Where(order => order.Status == "Delivered")
                            .GroupBy(order => new { orderDate = order.OrderDate.Value.Month, orderDateYear = order.OrderDate.Value.Year })
                            .Select(group => new { Month = group.Key.orderDate, Year = group.Key.orderDateYear, Count = group.Count() })
                            .OrderBy(o => o.Year)
                            
                            .ToList();
            var values = new ObservableCollection<BarInfo>();
            foreach (var o in ordersByMonth)
            {
                values.Add(new BarInfo
                {
                    Category = o.Month.ToString() + "/" + o.Year.ToString(),
                    Value = o.Count
                });
            }
            return values;
        }

        private string getTotalCustomer()
        {
            return _shopContext.Customers.Count().ToString();
        }
        private string getTotalDelivering()
        {
            return _shopContext.Orders.Where(o => o.Status == "Delivering").Count().ToString();
        }
        private string getLastMonthRevenue()
        {
            var query = from o in _shopContext.Orders
                        .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month - 1 && o.OrderDate.Value.Year == DateTime.Now.Year)
                        select o.TotalAmount;
            return query.Sum().ToString();
        }
        private string getThisMonthRevenue()
        {
            var query = from o in _shopContext.Orders
                        .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month && o.OrderDate.Value.Year == DateTime.Now.Year)
                        select o.TotalAmount;
            return query.Sum().ToString();
        }
        private string getTotalAvailableProduct()
        {
            return _shopContext.Plants.Where(p => p.StockQuantity > 0).Count().ToString();
        }

        private string getTotalCustomerBuyThisMonth()
        {
            var query = _shopContext.Orders.Where(o => o.OrderDate.Value.Month == DateTime.Now.Month && o.OrderDate.Value.Year == DateTime.Now.Year)
                .Select(o => o.CustomerId).Distinct();
            return query.Count().ToString();
        }
    }
}
