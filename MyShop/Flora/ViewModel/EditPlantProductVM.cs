using System.Collections.Generic;

namespace Flora.ViewModel
{
    class EditPlantProductVM : Utilities.ViewModelBase
    {

        public Plant Plant { get; set; }
        public List<string> ProductStatus { get; set; }
        public List<PlantCategory> ProductTypes { get; set; }

        public EditPlantProductVM()
        {
            Plant = new Plant()
            {
                PlantId = 1,
                Category = new PlantCategory()
                {
                    CategoryImages = "Images/ProductTypes/1.png",
                    CategoryId = 1,
                    CategoryName = "Indoor Plants"
                },
                CategoryId = 1,
                Description = "This is a plant product",
                Name = "Plant Product",
                OrderDetails = null,
                PlantImage = "/Images/ProductTypes/1.png",
                Price = 100,
                StockQuantity = 10
            };
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
    }
}
