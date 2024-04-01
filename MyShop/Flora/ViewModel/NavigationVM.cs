using Flora.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Flora.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private readonly Stack<object> navigationHistory = new Stack<object>();
        public event EventHandler BeforeViewChange;
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != null)
                {
                    navigationHistory.Push(_currentView); // Save current view before changing
                }
                _currentView = value; OnPropertyChanged();
            }
        }
        public void NavigateBack()
        {
            if (navigationHistory.Any())
            {
                _currentView = navigationHistory.Pop();
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand VouchersCommand { get; set; }
        public ICommand PlantsCommand { get; set; }
        public ICommand AddProductCategoryCommand { get; set; }
        public ICommand AddPlantProductCommand { get; set; }
        public ICommand EditProductCategoryCommand { get; set; }
        public ICommand EditPlantProductCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Product(object obj) => CurrentView = new ProductVM();
        private void Order(object obj) => CurrentView = new OrderVM();
        private void Voucher(object obj) => CurrentView = new VoucherVM();
        private void Plant(object obj) => CurrentView = new PlantProductVM();
        private void AddPlantCategory(object obj) => CurrentView = new AddProductCategoryVM();
        private void AddPlantProduct(object obj) => CurrentView = new AddPlantProductVM();
        private void EditPlantCategory(object parameter)
        {
            if (parameter is PlantCategory category)
            {
                var viewModel = new EditProductCategoryVM(category);
                CurrentView = viewModel;
            }
        }
        private void EditPlantProduct(object obj) => CurrentView = new EditPlantProductVM();
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(ProductVM)));
            OrdersCommand = new RelayCommand(Order);
            VouchersCommand = new RelayCommand(Voucher);
            PlantsCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(PlantProductVM)));
            AddProductCategoryCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(AddProductCategoryVM)));
            AddPlantProductCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(AddPlantProductVM)));
            EditProductCategoryCommand = new RelayCommand(category =>
            {
                NavigateToWithParameter(typeof(EditProductCategoryVM), category);
            });
            EditPlantProductCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(EditPlantProductVM)));
            CurrentView = new HomeVM();

        }
        public void ChangeViewMethod(Type viewModelType)
        {
            BeforeViewChange?.Invoke(this, EventArgs.Empty);
            var viewModelInstance = Activator.CreateInstance(viewModelType);
            if (viewModelInstance != null)
                CurrentView = viewModelInstance;
        }
        // Add a method to navigate with parameters
        public void NavigateToWithParameter(Type viewModelType, object parameter)
        {
            BeforeViewChange?.Invoke(this, EventArgs.Empty);

            // Check if the ViewModel type has a constructor that accepts the parameter
            var constructor = viewModelType.GetConstructor(new Type[] { parameter.GetType() });
            if (constructor != null)
            {
                // Create an instance of the ViewModel using the constructor with parameter
                var viewModelInstance = constructor.Invoke(new object[] { parameter });
                if (viewModelInstance != null)
                {
                    CurrentView = viewModelInstance;
                }
            }
            else
            {
                throw new InvalidOperationException($"No suitable constructor found for ViewModel: {viewModelType.Name}");
            }
        }
    }
}
