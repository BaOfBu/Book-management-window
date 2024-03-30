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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
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
        Order _oldData;
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
            var screen = new AddOrder();
            if (screen.ShowDialog() == true)
            {
                Order newOrder = screen.GetNewOrder();

                MessageBox.Show("Insert an order successfully");
                orderVM.OrderList.Add(newOrder);
            }
        }
        private void UpdateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = gridView.SelectedItem as Order;
            if (selectedOrder != null)
            {
                _oldData = (Order)selectedOrder.Clone();

                var screen = new DetailOrder(selectedOrder);

                if (screen.ShowDialog() == true)
                {
                    var order = screen.GetOrder();

                    if (order != null)
                    {
                        MessageBox.Show("Update an order successfully");
                        for (int i = 0; i < orderVM.OrderList.Count; i++)
                        {
                            if ((orderVM.OrderList[i].OrderId == selectedOrder.OrderId))
                            {
                                orderVM.OrderList[i] = selectedOrder;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Remove the order successfully");
                        orderVM.OrderList.Remove(selectedOrder);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Choose an order");
            }
        }
        private void radDateRangePicker_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            if (DateTime.TryParse("2024-01-01", out DateTime start))
            {
                radDateRangePicker.StartDate = start;
            }
        }

        private void RemoveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)gridView.SelectedItem;
            orderVM.RemoveOrderCommand.Execute(selectedOrder);
        }
    }
}
