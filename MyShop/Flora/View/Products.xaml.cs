﻿using Flora.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void SearchBoxControl_Loaded(object sender, RoutedEventArgs e)
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
                    if (productVM != null)
                    {
                        productVM.PageSize = pageSize;
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

        }
        private void ImportFromExcel_Click(object sender, RoutedEventArgs e)
        {

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
                    string selectedName = selectedPlantType.CategoryName;
                    // Create a new instance of the PlantProduct UserControl
                    PlantProduct plantProduct = new PlantProduct();
                    // For example, if you have a ContentControl named "contentControl":
                    plantProduct.Show();
                }
            }
        }

        private void DataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            // e.OldPageIndex gives you the previous page index
            // e.NewPageIndex gives you the new current page index

            // Debug.WriteLine(e.OldPageIndex.ToString() + " " + e.NewPageIndex.ToString());
        }
    }
}
