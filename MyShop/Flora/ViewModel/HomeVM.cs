using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Flora.Model;
using System.Threading.Tasks;
using Flora.View;
using Microsoft.EntityFrameworkCore;

namespace Flora.ViewModel
{
    class HomeVM : Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        public ObservableCollection<HomeProductPreview> Plants { get; set; }
        public HomeVM()
        {
            _shopContext = new MyShopContext();
            Plants = get5LeastProduct();
            
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
    }
}
