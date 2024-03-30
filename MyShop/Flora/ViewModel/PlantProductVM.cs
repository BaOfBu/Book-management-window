using System.Collections.Generic;
using System.ComponentModel;

namespace Flora.ViewModel
{
    class PlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get; set; }
        public int PageSize { get; set; }

        public BindingList<Plant> PlantList { get; set; }
        public PlantProductVM()
        {
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {   "Sort by name ascending",
                                                    "Sort by name descending",
                                                 };
            PageSize = 8;
            PlantList = new BindingList<Plant> {
<<<<<<< HEAD
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 1, Name = "Indoor Plants",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 2, Name = "Outdoor Plants",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 3, Name = "Flowering Plants", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 4, Name = "Succulents",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 5, Name = "Herbs", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 6, Name = "Fruit Trees", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/ProductCategory01.png", PlantId = 7, Name = "Vegetables", Price = 100}
=======
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 1, Name = "Indoor Plants",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 2, Name = "Outdoor Plants",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 3, Name = "Flowering Plants", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 4, Name = "Succulents",Price=100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 5, Name = "Herbs", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 6, Name = "Fruit Trees", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100},
                    new Plant() { PlantImage = "Images/ProductTypes/1.png", PlantId = 7, Name = "Vegetables", Price = 100}
>>>>>>> main

            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
