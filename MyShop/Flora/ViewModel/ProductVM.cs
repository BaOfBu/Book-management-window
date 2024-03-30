
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class ProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public List<string> PagesNumberList { get; set; }
        public List<string> SortTypeList { get; set; }

        private int _pageSize = 8;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                    LoadPlants();
                }
            }
        }

        public int PageNumber { get; set; }

        public ObservableCollection<PlantCategory> PlantCategoryList { get; set; }

        public ObservableCollection<PlantCategory> AllPlantCategoryList { get; set; }

        public ProductVM()
        {
            PageSize = 8;
            PageNumber = 1;
            PagesNumberList = new List<string> { "8", "16", "24", "32", "64", "96" };
            SortTypeList = new List<string>  {  "Sort by name ascending",
                                                "Sort by name descending",
                                             };
            LoadPlants();
        }
        private async void LoadPlants()
        {
            AllPlantCategoryList = await GetAllPlantCategoriesAsync();
            PlantCategoryList = await GetPlantCategoriesAsync(PageNumber, PageSize);

        }

        public async Task<ObservableCollection<PlantCategory>> GetPlantCategoriesAsync(int pageNumber, int pageSize)
        {
            using (var context = new MyShopContext())
            {
                if (pageNumber < 1)
                    throw new ArgumentOutOfRangeException(nameof(pageNumber), "PageNumber must be greater than 0.");
                if (pageSize < 1)
                    throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0.");
                int skipAmount = (pageNumber - 1) * pageSize;
                List<PlantCategory> categories = await context.Set<PlantCategory>()
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync();
                return new ObservableCollection<PlantCategory>(categories);
            }
        }
        public async Task<ObservableCollection<PlantCategory>> GetAllPlantCategoriesAsync()
        {
            using (var context = new MyShopContext())
            {
                List<PlantCategory> categories = await context.Set<PlantCategory>()
                    .ToListAsync();
                return new ObservableCollection<PlantCategory>(categories);
            }
        }

    }
}
