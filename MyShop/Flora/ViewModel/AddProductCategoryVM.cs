using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class AddProductCategoryVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private int _nextCategoryId;
        private string _categoryName;
        private string _categoryImages;
        private MyShopContext _shopContext = new MyShopContext();
        public int NextCategoryId
        {
            get => _nextCategoryId;
            set
            {
                _nextCategoryId = value;
                OnPropertyChanged();
            }
        }
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }
        public string CategoryImages
        {
            get => _categoryImages;
            set
            {
                _categoryImages = value;
                OnPropertyChanged();
            }
        }
        public AddProductCategoryVM()
        {
            InitializeNextCategoryId();

        }
        private async void InitializeNextCategoryId()
        {
            NextCategoryId = await GetNextCategoryIdAsync();
        }
        public async Task<int> GetNextCategoryIdAsync()
        {
            var maxId = await _shopContext.PlantCategories.MaxAsync(c => (int?)c.CategoryId) ?? 0;
            return maxId + 1;
        }
        public async Task SaveCategoryAsync()
        {
            var newCategory = new PlantCategory
            {
                CategoryId = NextCategoryId,
                CategoryName = CategoryName,
                CategoryImages = CategoryImages,
                Plants = null
            };

            await _shopContext.PlantCategories.AddAsync(newCategory);
            await _shopContext.SaveChangesAsync();

            CategoryName = string.Empty;
            CategoryImages = string.Empty;
            InitializeNextCategoryId();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}