using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.Linq;
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
        public PlantProduct(PlantCategory plantCategory) : this()
        {

            DataContext = new EditProductCategoryVM(plantCategory);
            plantProductVM = DataContext as PlantProductVM;

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
                var viewModel = DataContext as PlantProductVM;
                if (int.TryParse(selectedItem, out pageSize))
                {
                    if (viewModel != null)
                    {

                        viewModel.PageSize = pageSize;
                        dataPager.PageIndex = 0;
                    }
                }
            }
        }


        private void SortTypeList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                string selectedSortType = listBox.SelectedItem.ToString();
                if (DataContext is PlantProductVM viewModel)
                {
                    dataPager.PageIndex = 0;
                    viewModel.CurrentSortOrder = selectedSortType;
                }
            }
        }

        private void AddNewProductType_Click(object sender, RoutedEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.AddPlantProductCommand.Execute(null);
            }
        }
        private async void ImportFromExcel_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                    PlantProductVM viewModel = DataContext as PlantProductVM;
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

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.ProductsCommand.Execute(null);
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
                    var navigationVM = GetNavigationVMFromMainWindow();

                    if (navigationVM != null)
                    {
                        navigationVM.EditPlantProductCommand.Execute(selectedPlantType);
                    }
                }
            }
        }
        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            var viewModel = DataContext as PlantProductVM;
            if (viewModel != null)
            {
                viewModel.PageNumber = e.NewPageIndex + 1;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ViewModel is not available.");
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
                        navigationVM.EditPlantProductCommand.Execute(item);
                    }
                }
            }
        }

        private void RadSlider_SelectionChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            decimal newStartValue = (decimal)RadSlider1.SelectionStart;
            decimal newEndValue = (decimal)RadSlider1.SelectionEnd;
            var viewModel = DataContext as PlantProductVM;
            if (viewModel != null)
            {
                dataPager.PageIndex = 0;
                viewModel.MinimumPrice = newStartValue;
                viewModel.MaximumPrice = newEndValue;
            }
        }

        private void txtSearchOrders_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataPager.PageIndex = 0;
        }
    }
}