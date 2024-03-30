using Flora.Model;
using Flora.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for DetailOrder.xaml
    /// </summary>
    public partial class DetailOrder : Window
    {
        private DetailOrderVM detailOrderVM { get; set; }
        public Order info {  get; set; }
        public DetailOrder()
        {

        }
        public DetailOrder(Order selectedOrder)
        {
            info = (Order)selectedOrder.Clone();
            detailOrderVM = new DetailOrderVM(info);
            InitializeComponent();
            DataContext = detailOrderVM;
        }
        public Order GetOrder()
        {
            return detailOrderVM.SelectedOrder;
        }
        private void ComboBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            detailOrderVM.ComboBoxItemSelectionChangedCommand.Execute(comboBox.DataContext);
        }
        private void ComboBoxQuantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            detailOrderVM.ComboBoxQuantitySelectionChangedCommand.Execute(comboBox.DataContext);
        }
        private void ComboBoxVoucher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            detailOrderVM.ComboBoxVoucherSelectionChangedCommand.Execute(comboBox.SelectedItem);
        }
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            detailOrderVM.AddItemPanelCommand.Execute(null);
        }
        private void RemoveRadButton_Click(object sender, RoutedEventArgs e)
        {
            detailOrderVM.RemoveOrderCommand.Execute(null);
            DialogResult = true;
        }
        private void UpdateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer()
            {
                Name = customerName.Text,
                Email = customerEmail.Text,
                Phone = customerPhoneNumber.Text,
                Address = customerDeliveryAddress.Text,
            };

            detailOrderVM.CreateOrderCommand.Execute(customer);
            DialogResult = true;
        }
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ItemViewModel)ItemsListView.SelectedItem;
            if (selectedItem != null)
            {
                detailOrderVM.RemoveItemPanelCommand.Execute(selectedItem);
            }
        }

        private void ComboBoxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            detailOrderVM.ComboBoxStatusSelectionChangedCommand.Execute(comboBox.SelectedItem);
        }
    }
}
