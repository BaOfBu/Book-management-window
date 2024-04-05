using System;
using System.Linq;
using System.Windows;

namespace Flora.ViewModel
{
    class EditProductCategoryVM : Utilities.ViewModelBase
    {
        private PlantCategory _plantCategory;
        private MyShopContext _shopContext;
        public string categoryName { get; set; }

        public string categoryImage { get; set; }
        public PlantCategory PlantCategory
        {
            get => _plantCategory;
            set
            {
                _plantCategory = value;

                OnPropertyChanged(nameof(PlantCategory));
            }
        }

        public EditProductCategoryVM(PlantCategory category)
        {
            _shopContext = new MyShopContext();
            categoryName = category.CategoryName;
            categoryImage = category.CategoryImages;
            PlantCategory = category;
        }

        public void UpdateCategoryInDatabase()
        {
            try
            {
                // Retrieve the original category from the database
                var originalCategory = _shopContext.PlantCategories.FirstOrDefault(c => c.CategoryId == PlantCategory.CategoryId);

                if (originalCategory != null)
                {
                    // Update the properties of the original category with the modified values
                    originalCategory.CategoryName = PlantCategory.CategoryName;
                    categoryName = PlantCategory.CategoryName;
                    // Save changes to the database
                    _shopContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Category not found in the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating category: {ex.Message}");
            }
        }
        public void DeleteCategoryFromDatabase()
        {
            try
            {
                var categoryToDelete = _shopContext.PlantCategories.FirstOrDefault(c => c.CategoryId == PlantCategory.CategoryId);

                if (categoryToDelete != null)
                {
                    _shopContext.PlantCategories.Remove(categoryToDelete);

                    _shopContext.SaveChanges();

                    MessageBox.Show("Category has been successfully deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Category not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
