using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class PlantVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private MyShopContext _shopContext;

        public List<PlantCategory> PlantCategories { get; set; }

        public ObservableCollection<Plant> Plants { get; set; }

        private int _pageSize = 8;
        private int _pageNumber = 1;
        private int _totalItemCount = 0;
        private string _currentSortOrder = string.Empty;
        private string _searchText = string.Empty;
        public List<string> PagesNumberList { get; } = new List<string> { "8", "16", "24", "32", "64", "96" };
        public List<string> SortTypeList { get; } = new List<string> {
            "Sort by name ascending",
            "Sort by name descending",
            "Sort by price ascending",
            "Sort by price descending"
        };

        public string CurrentSortOrder
        {
            get => _currentSortOrder;
            set
            {
                if (_currentSortOrder != value)
                {
                    _pageNumber = 1;
                    _currentSortOrder = value;
                    OnPropertyChanged(nameof(CurrentSortOrder));
                    LoadPlantAsync();
                }
            }
        }
        private decimal? _minimumPrice;
        public decimal? MinimumPrice
        {
            get => _minimumPrice;
            set
            {
                if (_minimumPrice != value)
                {
                    _pageNumber = 1;
                    _minimumPrice = value;
                    OnPropertyChanged(nameof(MinimumPrice));
                    LoadPlantAsync();
                }
            }
        }

        private decimal? _maximumPrice;
        public decimal? MaximumPrice
        {
            get => _maximumPrice;
            set
            {
                if (_maximumPrice != value)
                {
                    _pageNumber = 1;
                    _maximumPrice = value;
                    OnPropertyChanged(nameof(MaximumPrice));
                    LoadPlantAsync();
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
                    LoadPlantAsync();
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
                    LoadPlantAsync();
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
                    LoadPlantAsync();
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
                    LoadPlantAsync();
                }
            }
        }
        public PlantVM()
        {
            Plants = new ObservableCollection<Plant>();
            _shopContext = new MyShopContext();
            LoadPlantAsync();

        }

        private async void LoadPlantAsync()
        {
            try
            {
                Plants.Clear();
                Plants = await LoadAllPlantsAsync(_pageNumber, _pageSize);
                TotalItemCount = await CalculateTotalItemCountAsync();
                LoadPlantCategoriesAsync();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task<ObservableCollection<Plant>> LoadAllPlantsAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            IQueryable<Plant> query = _shopContext.Plants;

            // Filter categories based on SearchText
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "")
            {
                query = query.Where(c => c.Name.Contains(SearchText));
            }

            // Filter by price range
            if (_minimumPrice.HasValue)
            {
                query = query.Where(p => p.Price >= _minimumPrice);
            }
            if (_maximumPrice.HasValue)
            {
                query = query.Where(p => p.Price <= _maximumPrice);
            }

            switch (CurrentSortOrder)
            {
                case "Sort by name ascending":
                    query = query.OrderBy(c => c.Name);
                    break;
                case "Sort by name descending":
                    query = query.OrderByDescending(c => c.Name);
                    break;
                case "Sort by price ascending":
                    query = query.OrderBy(c => c.Price);
                    break;
                case "Sort by price descending":
                    query = query.OrderByDescending(c => c.Price);
                    break;
                default:
                    break;
            }
            var plants = await query
                                    .Skip(skip)
                                    .Take(pageSize)
                                    .ToListAsync();
            return new ObservableCollection<Plant>(plants);
        }
        private async void LoadPlantCategoriesAsync()
        {
            try
            {
                PlantCategories = await _shopContext.PlantCategories.ToListAsync();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task<int> CalculateTotalItemCountAsync()
        {

            IQueryable<Plant> query = _shopContext.Plants;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.Contains(SearchText));
            }

            // Filter by price range
            if (_minimumPrice.HasValue)
            {
                query = query.Where(p => p.Price >= _minimumPrice);
            }
            if (_maximumPrice.HasValue)
            {
                query = query.Where(p => p.Price <= _maximumPrice);
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
