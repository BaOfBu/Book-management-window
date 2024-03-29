using System.ComponentModel;

namespace Flora.ViewModel
{
    class AddProductCategoryVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        public AddProductCategoryVM()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
