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

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for AddVoucher.xaml
    /// </summary>
    public partial class AddVoucher : Window
    {
        private AddVoucherVM addVoucherVM { get; set; }
        public AddVoucher()
        {
            addVoucherVM = new AddVoucherVM();
            InitializeComponent();
            DataContext = addVoucherVM;
        }

        private void CreateVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            Coupon coupon = new Coupon() { 
                CouponCode = ((TextBox)couponNameField.Content).Text,
                Discount = decimal.Parse(((TextBox)discountField.Content).Text),
                StartDate = DateOnly.FromDateTime((DateTime)validateTimeField.StartDate),
                ExpiryDate = DateOnly.FromDateTime((DateTime)validateTimeField.EndDate),
                Status = comboBoxStatus.SelectedItem.ToString(),
            };

            addVoucherVM.CreateVoucherCommand.Execute(coupon);
            DialogResult = true;
        }

        private void ClearRadButton_Click(object sender, RoutedEventArgs e)
        {
            ((TextBox)couponNameField.Content).Text = string.Empty;
            ((TextBox)discountField.Content).Text = string.Empty;
            validateTimeField.StartDate = default;
            validateTimeField.EndDate = default;
            comboBoxStatus.SelectedIndex = -1;
        }

        public Coupon getNewCoupon()
        {
            return addVoucherVM.newCoupon;
        }
    }
}
