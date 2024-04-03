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
        private int _totalItemCount = 0;
        private string _currentSortOrder = string.Empty;
        private string _searchText = string.Empty;
        public List<string> PagesNumberList { get; } = new List<string> { "8", "16", "24", "32", "64", "96" };
        public List<string> SortTypeList { get; } = new List<string> { "Sort by name ascending", "Sort by name descending" };
        public ObservableCollection<PlantCategory> PlantCategoryList { get; set; }

        public string CurrentSortOrder
        {
            get => _currentSortOrder;
            set
            {
                if (_currentSortOrder != value)
                {
                    _currentSortOrder = value;
                    _pageNumber = 1;
                    OnPropertyChanged(nameof(CurrentSortOrder));
                    LoadPlantCategoryAsync();
                }
            }
        }

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

        public ProductVM()
        {
            PlantCategoryList = new ObservableCollection<PlantCategory>();
            LoadPlantCategoryAsync();
        }

        private async void LoadPlantCategoryAsync()
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

        public async Task<ObservableCollection<PlantCategory>> LoadAllPlantCategoriesAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<PlantCategory> query = _shopContext.PlantCategories;

            // Filter categories based on SearchText
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(c => c.CategoryName.Contains(SearchText));
            }
            switch (CurrentSortOrder)
            {
                case "Sort by name ascending":
                    query = query.OrderBy(c => c.CategoryName);
                    break;
                case "Sort by name descending":
                    query = query.OrderByDescending(c => c.CategoryName);
                    break;
                default:
                    break;
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
