using Flora.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Flora.ViewModel
{
    class ProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get; set; }

        public int PageSize { get; set; }

        public BindingList<PlantType> PlantTypesList { get; set; }

        public ProductVM()
        {
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {   "Sort by name ascending",
                                                    "Sort by name descending",
                                                 };
            PageSize = 8;
            PlantTypesList = new BindingList<PlantType> {
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 1, Name = "Indoor Plants"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 2, Name = "Outdoor Plants"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 3, Name = "Flowering Plants"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 4, Name = "Succulents"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 5, Name = "Herbs"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 6, Name = "Fruit Trees"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"},
                    new PlantType(){Image ="Images/ProductTypes/1.png", ID = 7, Name = "Vegetables"}

                };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
