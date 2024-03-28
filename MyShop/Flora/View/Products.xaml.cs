using Flora.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

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
        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            // e.OldPageIndex gives you the previous page index
            // e.NewPageIndex gives you the new current page index

            // Debug.WriteLine(e.OldPageIndex.ToString() + " " + e.NewPageIndex.ToString());
        }
    }
}
