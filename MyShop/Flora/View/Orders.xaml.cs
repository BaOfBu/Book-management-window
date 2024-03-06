using Flora.Model;
using Flora.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed.UI;
using Telerik.Windows.Documents.FormatProviders.Html.Parsing.Dom;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        private OrderVM orderVM { get; set; }
        public Orders()
        {
            InitializeComponent();
            orderVM = DataContext as OrderVM;
        }

        private void SelectedListBoxItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedListBoxItem = (sender as ListBox).SelectedItem;

            if (selectedListBoxItem != null && selectedListBoxItem is string)
            {
                string selectedItem = selectedListBoxItem as string;

                ResultsPerPage.Content = selectedItem;

                int pageSize;
                if (int.TryParse(selectedItem, out pageSize))
                {
                    if (orderVM != null)
                    {
                        orderVM.PageSize = pageSize;
                    }
                }
            }
        }
        private void AddAnOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrder addOrderWindow = new AddOrder();
            addOrderWindow.Show();
        }

        private void UpdateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            DetailOrder detailOrder = new DetailOrder();
            detailOrder.Show();
        }
    }
}
