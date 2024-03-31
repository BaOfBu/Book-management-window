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
            customerName.Text = string.Empty;
            customerEmail.Text = string.Empty;
            customerPhoneNumber.Text = string.Empty;
            customerDeliveryAddress.Text = string.Empty;

            comboBoxVouchers.SelectedIndex = -1;

            addOrderVM.ClearRadButtonCommand.Execute(null);
        }
        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            addOrderVM.CreateOrderCommand.Execute(null);
            DialogResult = true;
        }
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ItemViewModel)ItemsListView.SelectedItem;
            if(selectedItem != null)
            {
                addOrderVM.RemoveItemPanelCommand.Execute(selectedItem);
            }
            else
            {
                MessageBox.Show("Please choose a item", "Error");
            }
        }

    }
}
