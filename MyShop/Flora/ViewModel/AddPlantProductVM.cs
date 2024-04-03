using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class AddPlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private MyShopContext _shopContext;

        private ObservableCollection<PlantCategory> _productTypes;
        public ObservableCollection<PlantCategory> ProductTypes
        {
            get => _productTypes;
            set
            {
                _productTypes = value;
                OnPropertyChanged();
            }
        }
        private string _name;
        private string _description;
        private int _stockQuantity;
        private decimal _price;
        private int _nextPlantId;
        private int _categoryId;
        private string _plantImage;

        public int NextPlantId
        {
            get => _nextPlantId;
            set
            {
                _nextPlantId = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set
            {
                _stockQuantity = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                OnPropertyChanged();
            }
        }

        public string PlantImage
        {
            get => _plantImage;
            set
            {
                _plantImage = value;
                OnPropertyChanged();
            }
        }
        public AddPlantProductVM()
        {
            _shopContext = new MyShopContext();
            ProductTypes = new ObservableCollection<PlantCategory>();
            _ = InitializeNextCategoryIdAsync();
            _ = LoadProductTypesAsync();
        }
        private async Task InitializeNextCategoryIdAsync()
        {
            NextPlantId = await GetNextPlantIdAsync();
        }

        public async Task<int> GetNextPlantIdAsync()
        {
            var maxId = await _shopContext.Plants.MaxAsync(c => (int?)c.PlantId) ?? 0;
            return maxId + 1;
        }

        private async Task LoadProductTypesAsync()
        {
            var categories = await _shopContext.PlantCategories
                .ToListAsync();

            // Assuming ProductTypes is an ObservableCollection<PlantCategory>
            foreach (var category in categories)
            {
                ProductTypes.Add(category);
            }
        }
        public async Task SavePlantAsync()
        {
            var newPlant = new Plant
            {
                PlantId = this.NextPlantId,
                Name = this.Name,
                Price = this.Price,
                Description = this.Description,
                StockQuantity = this.StockQuantity,
                CategoryId = this.CategoryId,
                PlantImage = this.PlantImage
            };

            try
            {
                _shopContext.Plants.Add(newPlant);
                await _shopContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the plant: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}