using Flora.Model;
using Flora.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Telerik.Windows.Diagrams.Core;
using static Azure.Core.HttpHeader;
using Label = Telerik.Windows.Controls.Label;

namespace Flora.View
{
    public partial class AddOrder : Window
    {
        private AddOrderVM addOrderVM { get; set; }
        public AddOrder()
        {
            addOrderVM = new AddOrderVM();
            InitializeComponent();
            DataContext = addOrderVM;
        }
        public Order GetNewOrder()
        {
            return addOrderVM.NewOrder;
        }
        private void ComboBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            addOrderVM.ComboBoxItemSelectionChangedCommand.Execute(comboBox.DataContext);
        }
        private void ComboBoxQuantity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            addOrderVM.ComboBoxQuantitySelectionChangedCommand.Execute(comboBox.DataContext);
        }
        private void ComboBoxVoucher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            addOrderVM.ComboBoxVoucherSelectionChangedCommand.Execute(comboBox.SelectedItem);
        }
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            addOrderVM.AddItemPanelCommand.Execute(null);
        }
        private void ClearRadButton_Click(object sender, RoutedEventArgs e)
        {
            ((TextBox)customerName.Content).Text = string.Empty;
            ((TextBox)customerEmail.Content).Text = string.Empty;
            ((TextBox)customerPhoneNumber.Content).Text = string.Empty;
            ((TextBox)customerDeliveryAddress.Content).Text = string.Empty;

            comboBoxVouchers.SelectedIndex = -1;

            addOrderVM.ClearRadButtonCommand.Execute(null);
        }
        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer()
            {
                Name = ((TextBox)customerName.Content).Text,
                Email = ((TextBox)customerEmail.Content).Text,
                Phone = ((TextBox)customerPhoneNumber.Content).Text,
                Address = ((TextBox)customerDeliveryAddress.Content).Text,
            };

            addOrderVM.CreateOrderCommand.Execute(customer);
            DialogResult = true;
        }
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ItemViewModel)ItemsListView.SelectedItem;
            if(selectedItem != null)
            {
                addOrderVM.RemoveItemPanelCommand.Execute(selectedItem);
            }
        }

    }
}
