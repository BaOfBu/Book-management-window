using Flora.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for PlantProduct.xaml
    /// </summary>
    public partial class PlantProduct : UserControl
    {


        private PlantProductVM plantProductVM { get; set; }

        public PlantProduct()
        {
            InitializeComponent();
            plantProductVM = DataContext as PlantProductVM;
        }
        private void SearchBoxControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        private void SelectedListBoxItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedListBoxItem = (sender as ListBox).SelectedItem;

            if (selectedListBoxItem != null && selectedListBoxItem is string)
            {
                string selectedItem = selectedListBoxItem as string;

                // Create a new TextBlock
                TextBlock textBlock = new TextBlock();
                textBlock.Text = selectedItem;
                textBlock.FontSize = 16;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Margin = new Thickness(10, 0, 10, 0);
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;

                // Set the Content of ResultsPerPage to the newly created TextBlock
                ResultsPerPage.Content = textBlock;

                int pageSize = -1;

                if (int.TryParse(selectedItem, out pageSize))
                {
                    if (plantProductVM != null)
                    {
                        plantProductVM.PageSize = pageSize;
                    }
                }
            }
        }

        private void txtSearchOrders_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewProductType_Click(object sender, RoutedEventArgs e)
        {

            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.AddPlantProductCommand.Execute(null);
            }
        }
        private void ImportFromExcel_Click(object sender, RoutedEventArgs e)
        {
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.NavigateBack();
            }
        }
        private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Cast sender to ListViewItem to access properties
            ListViewItem listViewItem = sender as ListViewItem;

            // Retrieve the name of the selected item
            if (listViewItem != null && listViewItem.Content != null)
            {
                // Assuming PlantType is the type of the items in PlantTypesList
                Plant selectedPlantType = listViewItem.Content as Plant;

                if (selectedPlantType != null)
                {
                    string selectedName = selectedPlantType.Name;
                    PlantProduct plantProduct = new PlantProduct();
                    var navigationVM = GetNavigationVMFromMainWindow();

                    if (navigationVM != null)
                    {
                        navigationVM.EditPlantProductCommand.Execute(null);
                    }
                }
            }
        }

        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            // e.OldPageIndex gives you the previous page index
            // e.NewPageIndex gives you the new current page index

            // Debug.WriteLine(e.OldPageIndex.ToString() + " " + e.NewPageIndex.ToString());
        }
        private NavigationVM GetNavigationVMFromMainWindow()
        {
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow.DataContext is NavigationVM navigationVM)
            {
                return navigationVM;
            }
            return null;
        }

        private void MoreDetail_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is ListViewItem listViewItem)
            {
                var item = listViewItem.Content as Plant;

                if (item != null)
                {
                    var navigationVM = GetNavigationVMFromMainWindow();
                    if (navigationVM != null)
                    {
                        navigationVM.EditPlantProductCommand.Execute(null);
                    }
                }
            }
        }
    }
}