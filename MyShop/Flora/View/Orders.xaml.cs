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
            _oldData = new Order();
            orderVM = new OrderVM();
            InitializeComponent();
            DataContext = orderVM;
        }
        private void SelectedListBoxItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedListBoxItem = (sender as ListBox).SelectedItem;

            if (selectedListBoxItem != null && selectedListBoxItem is string)
            {
                string selectedItem = selectedListBoxItem as string;

                if (int.TryParse(selectedItem, out int pageSize))
                {
                    orderVM.PageSizeChangedCommand.Execute(pageSize);
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
                    Reflection.CopyProperties(order, selectedOrder);

                    if (order != null)
                    {
                        System.Windows.MessageBox.Show("Update an order successfully");
                        for (int i = 0; i < orderVM.OrderList.Count; i++)
                        {
                            if ((orderVM.OrderList[i].OrderId == selectedOrder.OrderId))
                            {
                                orderVM.OrderList[i] = order;
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
                else
                {
                    Reflection.CopyProperties(_oldData, selectedOrder);
                }

            }
            else
            {
                MessageBox.Show("Choose an order");
            }
        }
        private void RemoveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = (Order)gridView.SelectedItem;
            orderVM.RemoveOrderCommand.Execute(selectedOrder);
        }
        private void RadDateTimePickerStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = (RadDateTimePicker)sender;
            if (selectedDate.SelectedDate != null)
            {
                orderVM.StartDateChangedCommand.Execute(selectedDate.SelectedDate);
            }
        }
        private void RadDateTimePickerEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var endDate = (RadDateTimePicker)sender;
            if (endDate.SelectedDate != null)
            {
                orderVM.EndDateChangedCommand.Execute(endDate.SelectedDate);
            }
        }
        private void FilterRadButton_Click(object sender, RoutedEventArgs e)
        {
            orderVM.FilterOrderCommand.Execute(null);
            dataPager.PageIndex = 0;
        }
        private void ReloadRadButton_Click(object sender, RoutedEventArgs e)
        {
            radDateTimePicker_Start.SelectedValue = default;
            radDateTimePicker_End.SelectedValue = default;
            orderVM.ReloadOrderCommand.Execute(null);
            dataPager.PageIndex = 0;
        }

        private void dataPager_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            int pageIndex = e.NewPageIndex + 1;
            orderVM.LoadDataChangedCommand.Execute(pageIndex);
        }
        private void txtSearchOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchHandle();
            }
        }
        private void SearchHandle()
        {
            string keyword = txtSearchOrders.Text.Trim();

            orderVM.SearchOrderCommand.Execute(keyword);
            dataPager.PageIndex = 0;
        }
    }
}