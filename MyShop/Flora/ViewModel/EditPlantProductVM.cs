using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class EditPlantProductVM : Utilities.ViewModelBase
    {

        private Plant _plant;
        private MyShopContext _shopContext;
        public int? previous_id { get; set; }
        private List<PlantCategory> _productTypes;
        public Plant Plant
        {
            get => _plant;
            set
            {
                _plant = value;
                OnPropertyChanged(nameof(Plant));
            }
        }
        public List<PlantCategory> ProductTypes
        {
            get => _productTypes;
            set
            {
                _productTypes = value;
                OnPropertyChanged(nameof(ProductTypes));
            }
        }
        public EditPlantProductVM(Plant plant)
        {
            _shopContext = new MyShopContext();
            Plant = plant;
            previous_id = Plant.CategoryId;
            LoadProductTypesAsync();
        }
        public PlantCategory FindPlantCategoryObject(int? id)
        {
            try
            {
                return FindPlantCategoryByIdAsync(id).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while finding PlantCategory object: {ex.Message}");
                return null;
            }
        }
        public async Task LoadProductTypesAsync()
        {
            try
            {
                var productTypesList = await _shopContext.PlantCategories.ToListAsync();
                ProductTypes = productTypesList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading product types: {ex.Message}");
            }
        }
        public async Task<PlantCategory> FindPlantCategoryByIdAsync(int? id)
        {
            try
            {
                return await _shopContext.PlantCategories.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while finding PlantCategory by ID: {ex.Message}");
                return null;
            }
        }
        public async Task SaveChangesAsync()
        {
            try
            {
                _shopContext.Entry(Plant).State = EntityState.Modified;
                await _shopContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
            }
        }


    }
}
