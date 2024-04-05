using Flora.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Flora.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        public event EventHandler BeforeViewChange;

        private Stack<object> navigationHistory = new Stack<object>();

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
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
        public ICommand AllPlantCommand { get; set; }
        private void Home(object obj) => CurrentView = new HomeVM();
        private void Product(object obj)
        {
            CurrentView = new ProductVM();
        }
        private void AllPlant(object obj)
        {
            CurrentView = new PlantVM();
        }
        private void Order(object obj) => CurrentView = new OrderVM();
        private void Voucher(object obj) => CurrentView = new VoucherVM();
        private void Plant(object parameter)
        {
            if (parameter is PlantCategory category)
            {
                var viewModel = new PlantProductVM(category);
                CurrentView = viewModel;
            }
        }
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
        private void EditPlantProduct(object parameter)
        {
            if (parameter is Plant plant)
            {
                var viewModel = new EditPlantProductVM(plant);
                CurrentView = viewModel;
            }
        }
        public NavigationVM()
        {

            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(ProductVM)));
            OrdersCommand = new RelayCommand(Order);
            VouchersCommand = new RelayCommand(Voucher);
            AllPlantCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(PlantVM)));
            PlantsCommand = new RelayCommand(category =>
            {
                NavigateToWithParameter(typeof(PlantProductVM), category);
            });
            AddProductCategoryCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(AddProductCategoryVM)));
            AddPlantProductCommand = new RelayCommand(param => this.ChangeViewMethod(typeof(AddPlantProductVM)));
            EditProductCategoryCommand = new RelayCommand(category =>
            {
                NavigateToWithParameter(typeof(EditProductCategoryVM), category);
            });
            EditPlantProductCommand = new RelayCommand(plant =>
            {
                NavigateToWithParameter(typeof(EditPlantProductVM), plant);
            });
            CurrentView = new HomeVM();

        }
        public void ChangeViewMethod(Type viewModelType)
        {
            BeforeViewChange?.Invoke(this, EventArgs.Empty);
            var viewModelInstance = Activator.CreateInstance(viewModelType);
            if (viewModelInstance != null)
            {
                navigationHistory.Push(CurrentView);
                CurrentView = viewModelInstance;
            }

        }
        // Add a method to navigate with parameters
        public void NavigateToWithParameter(Type viewModelType, object parameter)
        {
            BeforeViewChange?.Invoke(this, EventArgs.Empty);
            if (parameter != null)
            {
                var constructor = viewModelType.GetConstructor(new Type[] { parameter.GetType() });
                if (constructor != null)
                {
                    var viewModelInstance = constructor.Invoke(new object[] { parameter });
                    if (viewModelInstance != null)
                    {
                        navigationHistory.Push(CurrentView);
                        CurrentView = viewModelInstance;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"No suitable constructor found for ViewModel: {viewModelType.Name}");
                }
            }
            else
            {

                var viewModelInstance = Activator.CreateInstance(viewModelType);
                if (viewModelInstance != null)
                    CurrentView = viewModelInstance;
            }

        }
        public void NavigateBack()
        {
            if (navigationHistory.Count > 0)
            {
                // Pop the previous view from the navigation history stack
                object previousView = navigationHistory.Pop();

                // Set the current view to the previous view
                CurrentView = previousView;
            }
        }

    }
}
