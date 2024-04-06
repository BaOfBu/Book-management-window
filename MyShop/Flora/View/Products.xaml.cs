using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            productVM = DataContext as ProductVM;
            if (productVM != null)
            {
                // Now it's safe to interact with productVM
                Debug.WriteLine(productVM.TotalItemCount);
            }
            else
            {
                Debug.WriteLine("ViewModel is not available.");
            }
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
                    if (productVM != null)
                    {
                        dataPager.PageIndex = 0;
                        productVM.PageSize = pageSize;
                    }
                }
            }
        }
        private void AddNewProductType_Click(object sender, RoutedEventArgs e)
        {
            AddProductCategory addPlantProduct = new AddProductCategory();
            var navigationVM = GetNavigationVMFromMainWindow();

            if (navigationVM != null)
            {
                navigationVM.AddProductCategoryCommand.Execute(null);
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
                PlantCategory selectedPlantType = listViewItem.Content as PlantCategory;

                if (selectedPlantType != null)
                {
                    var navigationVM = GetNavigationVMFromMainWindow();

                    if (navigationVM != null)
                    {
                        navigationVM.NavigateToWithParameter(typeof(PlantProductVM), selectedPlantType);
                    }
                }
            }

        }
        private NavigationVM GetNavigationVMFromMainWindow()
        {
            var mainWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.IsActive);
            if (mainWindow.DataContext is NavigationVM navigationVM)
            {
                return navigationVM;
            }
            return null;
        }

        private void DataPager_PageIndexChanged(object sender, Telerik.Windows.Controls.PageIndexChangedEventArgs e)
        {
            // Get the ProductVM from the DataContext
            var viewModel = DataContext as ProductVM;
            if (viewModel != null)
            {
                viewModel.PageNumber = e.NewPageIndex + 1;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ViewModel is not available.");
            }
        }

        private void MoreDetail_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is ListViewItem listViewItem)
            {
                var item = listViewItem.Content as PlantCategory;

                if (item != null)
                {
                    var navigationVM = GetNavigationVMFromMainWindow();

                    if (navigationVM != null)
                    {
                        navigationVM.NavigateToWithParameter(typeof(EditProductCategoryVM), item);
                    }
                }
            }
        }

        private void SortTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                string selectedSortType = listBox.SelectedItem.ToString();
                if (DataContext is ProductVM viewModel)
                {
                    dataPager.PageIndex = 0;
                    viewModel.CurrentSortOrder = selectedSortType;
                }
            }
        }

        private async void ImportFromExcel_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    // Call a method to read the Excel file and import data into the database
                    ProductVM viewModel = DataContext as ProductVM;
                    if (viewModel != null)
                    {
                        // Trigger the import operation in the ViewModel
                        await viewModel.ImportDataFromExcelAsync(filePath);
                    }

                }
                catch (Exception ex)
                {
                    // Handle any errors that may occur during the import process
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtSearchOrders_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataPager.PageIndex = 0;
        }

        private void RadRibbonDropDownButton_DropDownClosed(object sender, RoutedEventArgs e)
        {

        }

        private void RadRibbonDropDownButton_DropDownOpening(object sender, RoutedEventArgs e)
        {

        }

        private void RadRibbonDropDownButton_DropDownOpened(object sender, RoutedEventArgs e)
        {

        }
    }
}
