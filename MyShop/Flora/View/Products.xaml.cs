using Flora.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        private ProductVM productVM { get; set; }

        public Products()
        {
            InitializeComponent();
            productVM = DataContext as ProductVM;
        }

        private void SearchBoxControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearchOrders_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductTypeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
