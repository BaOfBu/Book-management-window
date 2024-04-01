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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Vouchers.xaml
    /// </summary>
    public partial class Vouchers : UserControl
    {
        private VoucherVM voucherVM { get; set; }
        Coupon _oldData;
        public Vouchers()
        {
            _oldData = new Coupon();
            voucherVM = new VoucherVM();
            InitializeComponent();
            DataContext = voucherVM;
        }
        private void SelectedListBoxItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedListBoxItem = (sender as ListBox).SelectedItem;

            if (selectedListBoxItem != null && selectedListBoxItem is string)
            {
                string selectedItem = selectedListBoxItem as string;

                ResultsPerPage.Content = selectedItem;

                if (int.TryParse(selectedItem, out int pageSize))
                {
                    if (voucherVM != null)
                    {
                        voucherVM.PageSize = pageSize;
                    }
                }
            }
        }
        private void AddAVoucher_Click(object sender, RoutedEventArgs e)
        {
            var screen = new AddVoucher();
            if(screen.ShowDialog() == true)
            {
                Coupon newCoupon = screen.getNewCoupon();

                MessageBox.Show("Insert a coupon successfully");
                voucherVM.CouponList.Add(newCoupon);
            }
        }
        private void UpdateCouponButton_Click(object sender, RoutedEventArgs e)
        {
            var coupon = gridView.SelectedItem as Coupon;
            if (coupon != null)
            {
                voucherVM.UpdateVoucherCommand.Execute(coupon);
                MessageBox.Show("Update the coupon successfully");
            }
            else
            {
                MessageBox.Show("Please choose a coupon");
            }
        }
        private void RemoveCouponButton_Click(object sender, RoutedEventArgs e)
        {
            Coupon selectedCoupon = (Coupon)gridView.SelectedItem;
            if (selectedCoupon.Status.Equals("Pending"))
            {
                voucherVM.RemoveVoucherCommand.Execute(selectedCoupon);
                MessageBox.Show("Remove a coupon successfully");
            }
            else
            {
                MessageBox.Show("Please choose a coupon in pending status");
            }
        }
        private void RadDateTimePickerStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDate = (RadDateTimePicker)sender;
            if (selectedDate.SelectedDate != null)
            {
                voucherVM.StartDateChangedCommand.Execute(selectedDate.SelectedDate);
            }
        }
        private void RadDateTimePickerEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var endDate = (RadDateTimePicker)sender;
            if (endDate.SelectedDate != null)
            {
                voucherVM.EndDateChangedCommand.Execute(endDate.SelectedDate);
            }
        }
        private void FilterRadButton_Click(object sender, RoutedEventArgs e)
        {
            voucherVM.FilterVoucherCommand.Execute(null);
        }
        private void ReloadRadButton_Click(object sender, RoutedEventArgs e)
        {
            radDateTimePicker_Start.SelectedValue = default;
            radDateTimePicker_End.SelectedValue = default;
            voucherVM.ReloadVoucherCommand.Execute(null);
        }
    }
}
