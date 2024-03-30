using System.Collections.Generic;
using System.ComponentModel;

namespace Flora.ViewModel
{
    class AddPlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<PlantCategory> ProductTypes { get; set; }
        public List<string> ProductStatus { get; set; }
        public AddPlantProductVM()
        {
            ProductTypes = new List<PlantCategory>
            {
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 1, CategoryName = "Indoor Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 2, CategoryName = "Outdoor Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 3, CategoryName = "Flowering Plants"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 4, CategoryName = "Succulents"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 5, CategoryName = "Herbs"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 6, CategoryName = "Fruit Trees"},
                    new PlantCategory() { CategoryImages = "Images/ProductTypes/1.png", CategoryId = 7, CategoryName = "Vegetables"},
            };
            ProductStatus = new List<string>
            {
                "Out of stock",
                "In stock",
                "Pre-order",
                "Arriving Soon"
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
