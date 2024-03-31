using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    public class ProductVM : INotifyPropertyChanged
    {
        private MyShopContext _shopContext = new MyShopContext();

        private int _pageSize = 8;
        private int _pageNumber = 1;

        public List<string> PagesNumberList { get; } = new List<string> { "8", "16", "24", "32", "64", "96" };
        public List<string> SortTypeList { get; } = new List<string> { "Sort by name ascending", "Sort by name descending" };

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                    LoadPlantsAsync();
                }
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged(nameof(PageNumber));
                    LoadPlantsAsync();
                }
            }
        }

        public ObservableCollection<PlantCategory> PlantCategoryList { get; set; }

        public ObservableCollection<PlantCategory> AllPlantCategoryList { get; set; }

        public ProductVM()
        {
            PlantCategoryList = new ObservableCollection<PlantCategory>();
            AllPlantCategoryList = new ObservableCollection<PlantCategory>();
            LoadPlantsAsync();
        }

        private async void LoadPlantsAsync()
        {
            try
            {
                // Load all categories only initially or when needed, not on every page change
                if (AllPlantCategoryList == null || !AllPlantCategoryList.Any())
                {
                    AllPlantCategoryList = await LoadAllPlantCategoriesAsync();
                }

                // Always update PlantCategoryList when LoadPlantsAsync is called
                PlantCategoryList = await LoadAllPlantCategoriesAsync(_pageNumber, _pageSize);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ObservableCollection<PlantCategory>> LoadAllPlantCategoriesAsync()
        {
            var categories = await _shopContext.PlantCategories.ToListAsync();
            return new ObservableCollection<PlantCategory>(categories);
        }

        public async Task<ObservableCollection<PlantCategory>> LoadAllPlantCategoriesAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var categories = await _shopContext.PlantCategories
                                  .Skip(skip)
                                  .Take(pageSize)
                                  .ToListAsync();
            return new ObservableCollection<PlantCategory>(categories);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
