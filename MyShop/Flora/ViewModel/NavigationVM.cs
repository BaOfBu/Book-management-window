using Flora.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flora.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand VouchersCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Product(object obj) => CurrentView = new ProductVM();
        private void Order(object obj) => CurrentView = new OrderVM();
        private void Voucher(object obj) => CurrentView = new VoucherVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(Product);
            OrdersCommand = new RelayCommand(Order);
            VouchersCommand = new RelayCommand(Voucher);

            CurrentView = new HomeVM();
        }
    }
}
