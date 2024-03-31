using Flora.Utilities;
using Flora.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class AddVoucherVM : Utilities.ViewModelBase
    {
        private readonly MyShopContext _shopContext;
        public Coupon newCoupon { get; set; }
        public string CouponCode { get; set; }
        public string Discount { get; set; }
        public List<string> Status { get; set; }
        public System.Windows.Input.ICommand CreateVoucherCommand { get; set; }
        public AddVoucherVM() {
            newCoupon = null;
            Status = new List<string>() { "Pending", "Active"};
            _shopContext = new MyShopContext();
            CreateVoucherCommand = new RelayCommand(CreateVoucher);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(CouponCode) || args.PropertyName == nameof(Discount))
                {
                    OnPropertyChanged(nameof(IsCreateCouponEnabled));
                }
            };
        }
        private void CreateVoucher(object parameter)
        {
            var coupon = parameter as Coupon;

            if (coupon != null)
            {
                if (_shopContext.Database.CanConnect())
                {
                    _shopContext.Coupons.Add(coupon);
                    _shopContext.SaveChanges();
                }
                int id = coupon.CouponId;
                newCoupon = coupon;
            }
        }
        public bool IsCreateCouponEnabled
            => !string.IsNullOrEmpty(CouponCode) &&
               CouponCodeRule.IsValidCouponCode(CouponCode) &&
               !string.IsNullOrEmpty(Discount) &&
               MoneyRule.IsValidMoney(Discount);
    }
}
