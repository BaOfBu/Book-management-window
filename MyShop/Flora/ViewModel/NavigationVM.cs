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

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Product(object obj) => CurrentView = new ProductVM();
        private void Order(object obj) => CurrentView = new OrderVM();
        private void Voucher(object obj) => CurrentView = new VoucherVM();
        private void Plant(object obj) => CurrentView = new PlantProductVM();
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);
            VouchersCommand = new RelayCommand(Voucher);
            PlantsCommand = new RelayCommand(param => this.ChangeViewMethodForPlant());
            CurrentView = new HomeVM();
        }
        public void ChangeViewMethodForPlant()
        {
            // Trigger the transition
            BeforeViewChange?.Invoke(this, EventArgs.Empty);

            // The actual view change happens after the animation. 
            // This will be handled by the subscriber of the BeforeViewChange event.
        }

    }
}
