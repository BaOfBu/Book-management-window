﻿using Flora.ViewModel;
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
            validateTimeField.StartDate = DateTime.Now;
            validateTimeField.EndDate = DateTime.Now;
        }

        private void CreateVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = (DateTime)validateTimeField.StartDate;
            DateTime endDate = (DateTime)validateTimeField.EndDate;

            if (startDate == default || startDate < DateTime.Now)
            {
                startDate = DateTime.Now;
            }

            if(endDate == default || endDate < startDate || endDate < DateTime.Now)
            {
                endDate = startDate.AddDays(7);
            }

            Coupon coupon = new Coupon() { 
                CouponCode = couponCode.Text,
                Discount = decimal.Parse(discount.Text),
                StartDate = DateOnly.FromDateTime(startDate),
                ExpiryDate = DateOnly.FromDateTime(endDate),
                Status = comboBoxStatus.SelectedItem.ToString(),
            };

            addVoucherVM.CreateVoucherCommand.Execute(coupon);
            DialogResult = true;
        }

        private void ClearRadButton_Click(object sender, RoutedEventArgs e)
        {
            couponCode.Text = string.Empty;
            discount.Text = string.Empty;
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
