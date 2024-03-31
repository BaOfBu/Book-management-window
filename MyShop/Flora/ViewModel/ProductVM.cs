using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

        private int _totalItemCount = 0;

        public int TotalItemCount
        {
            get => _totalItemCount;
            set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    OnPropertyChanged(nameof(TotalItemCount));
                    LoadPlantCategoryAsync();
                }
            }
        }
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageNumber = 1;
                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));
                    LoadPlantCategoryAsync();
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
                    LoadPlantCategoryAsync();
                }
            }
        }
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _pageNumber = 1;
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadPlantCategoryAsync();
                }
            }
        }


        public ObservableCollection<PlantCategory> PlantCategoryList { get; set; }

        public ProductVM()
        {
            PlantCategoryList = new ObservableCollection<PlantCategory>();
            LoadPagePlantsCategoryAsync();
        }
        private async void UpdateTotalItemCount()
        {
            TotalItemCount = await CalculateTotalItemCountAsync();
        }
        private async void LoadPlantCategoryAsync()
        {
            try
            {
                PlantCategoryList.Clear();
                PlantCategoryList = await LoadAllPlantCategoriesAsync(_pageNumber, _pageSize);
                if (SearchText == string.Empty || SearchText == "")
                {

                }
                TotalItemCount = await CalculateTotalItemCountAsync();
                Debug.WriteLine("TEST" + PageSize + "h" + TotalItemCount);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        private async void LoadPagePlantsCategoryAsync()
        {
            try
            {
                PlantCategoryList.Clear();
                PlantCategoryList = await LoadAllPlantCategoriesAsync(_pageNumber, _pageSize);
                TotalItemCount = await CalculateTotalItemCountAsync();

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
            IQueryable<PlantCategory> query = _shopContext.PlantCategories;

            // Filter categories based on SearchText
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(c => c.CategoryName.Contains(SearchText));
            }

            var categories = await query
                                    .Include(o => o.Plants)
                                    .Skip(skip)
                                    .Take(pageSize)
                                    .ToListAsync();
            return new ObservableCollection<PlantCategory>(categories);
        }

        public async Task<int> CalculateTotalItemCountAsync()
        {
            IQueryable<PlantCategory> query = _shopContext.PlantCategories;

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(c => c.CategoryName.Contains(SearchText));
            }
            else
            {
                return await query.CountAsync();
            }
            return await query.CountAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
