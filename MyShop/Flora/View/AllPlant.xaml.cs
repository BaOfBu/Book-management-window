using Flora.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for AllPlant.xaml
    /// </summary>
    public partial class AllPlant : UserControl
    {
        private PlantVM planttVM { get; set; }
        public AllPlant()
        {
            InitializeComponent();
            planttVM = DataContext as PlantVM;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SortTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem != null)
            {
                string selectedSortType = listBox.SelectedItem.ToString();
                if (DataContext is PlantVM viewModel)
                {
                    dataPager.PageIndex = 0;
                    viewModel.CurrentSortOrder = selectedSortType;
                }
            }
        }

        private void SelectedListBoxItem_Click(object sender, SelectionChangedEventArgs e)
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
                var viewModel = DataContext as PlantVM;
                if (int.TryParse(selectedItem, out pageSize))
                {
                    if (viewModel != null)
                    {
                        dataPager.PageIndex = 0;
                        viewModel.PageSize = pageSize;
                    }
                }
            }
        }

        private void RadSlider_SelectionChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            decimal newStartValue = (decimal)RadSlider1.SelectionStart;
            decimal newEndValue = (decimal)RadSlider1.SelectionEnd;
            var viewModel = DataContext as PlantVM;
            if (viewModel != null)
            {
                //dataPager.PageIndex = 0;
                viewModel.MinimumPrice = newStartValue;
                viewModel.MaximumPrice = newEndValue;
            }
        }

        private void AddNewPlantProduct_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var navigationVM = GetNavigationVMFromMainWindow();
            if (navigationVM != null)
            {
                navigationVM.AddPlantProductCommand.Execute(null);
            }
        }

        private void Category_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is PlantCategory selectedCategory)
            {
                dataPager.PageIndex = 0;
                CategoryButtonText.Text = textBlock.Text;

                if (planttVM != null)
                {
                    planttVM.SelectedCategory = selectedCategory;
                }
            }
        }

        private void MoreDetail_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ListViewItem listViewItem = FindParent<ListViewItem>(button);
                if (listViewItem != null && listViewItem.Content is Plant item)
                {
                    button.Tag = listViewItem;
                    var navigationVM = GetNavigationVMFromMainWindow();
                    if (navigationVM != null)
                    {
                        navigationVM.EditPlantProductCommand.Execute(item);
                    }
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            while (parentObject != null && !(parentObject is T))
            {
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }
            return parentObject as T;
        }

        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            var viewModel = DataContext as PlantVM;
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
            var mainWindow = Application.Current.MainWindow;
            if (mainWindow.DataContext is NavigationVM navigationVM)
            {
                return navigationVM;
            }
            return null;
        }

        private void txtSearchOrders_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataPager.PageIndex = 0;
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
                    PlantVM viewModel = DataContext as PlantVM;
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
    }
}
