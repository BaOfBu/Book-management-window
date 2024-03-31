
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Flora.ViewModel
{
    class PlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get; set; }
        public int PageSize { get; set; }

        public ObservableCollection<Plant> PlantList { get; set; }
        public PlantProductVM()
        {
            LoadPlants();
        }
        private async void LoadPlants()
        {
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {  "Sort by name ascending",
                                                "Sort by name descending",
                                             };
            PageSize = 8;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}