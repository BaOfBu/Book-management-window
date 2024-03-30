using System.Collections.Generic;
using System.ComponentModel;

namespace Flora.ViewModel
{
    class ProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get; set; }
        public int PageSize { get; set; }

        public BindingList<PlantCategory> PlantTypesList { get; set; }

        public ProductVM()
        {
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {   "Sort by name ascending",
                                                    "Sort by name descending",
                                                 };
            PageSize = 8;

            PlantTypesList = new BindingList<PlantCategory> {
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 1, CategoryName = "Indoor Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 2, CategoryName = "Outdoor Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 3, CategoryName = "Flowering Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 4, CategoryName = "Succulents"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 5, CategoryName = "Herbs"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 6, CategoryName = "Fruit Trees"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/ProductCategory01.png", CategoryId = 7, CategoryName = "Vegetables"}

    };
        }
    }
}
