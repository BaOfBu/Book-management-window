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

        public class PlotInfo
        {
            public double Category { get; set; }
            public double Value { get; set; }
        }

        //....... 
        public ObservableCollection<PlotInfo> Points { get; set; }
        public HomeVM()
        {
            Points = new ObservableCollection<PlotInfo>
            {
                new PlotInfo() { Category = 1, Value = 2},
                new PlotInfo() { Category = 2, Value = 3},
                new PlotInfo() { Category = 3, Value = 5},
                new PlotInfo() { Category = 4, Value = 7},
                new PlotInfo() { Category = 5, Value = 6},
                new PlotInfo() { Category = 6, Value = 8},
                new PlotInfo() { Category = 7, Value = 9},
                new PlotInfo() { Category = 8, Value = 10},
                new PlotInfo() { Category = 9, Value = 12},
                new PlotInfo() { Category = 10, Value = 13},
                new PlotInfo() { Category = 11, Value = 15},
                new PlotInfo() { Category = 12, Value = 16},
            };

            _shopContext = new MyShopContext();
            Plants = get5LeastProduct();
            //myBarSeries.ItemSource = Points;
            
        }   

        //private List<Plant> get5LeastProduct()
        //{
        //    var query = from p in _shopContext.Plants
        //                orderby p.StockQuantity
        //                select p;

        //    return query.Take(5).ToList();
        //}

        private ObservableCollection<HomeProductPreview> get5LeastProduct()
        {
            var query = _shopContext.Plants
                        .OrderBy(p => p.StockQuantity)
            .Include(p => p.Category);
            var products = query.ToList();
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
            //return new ObservableCollection<HomeProductPreview>();
        }
    }
}
