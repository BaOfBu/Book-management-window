using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class AddPlantProductVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private MyShopContext _shopContext;

        public List<PlantCategory> PlantCategories { get; set; }

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
            LoadData();


        }
        private async void LoadData()
        {
            try
            {
                _shopContext = new MyShopContext();
                NextPlantId = await GetNextPlantIdAsync();
                PlantCategories = await _shopContext.PlantCategories.ToListAsync();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task<int> GetNextPlantIdAsync()
        {
            _shopContext = new MyShopContext();
            var maxId = await _shopContext.Plants.MaxAsync(c => (int?)c.PlantId) ?? 0;
            return maxId + 1;
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