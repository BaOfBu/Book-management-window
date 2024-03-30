using Flora.Utilities;
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
        public List<string> Status { get; set; }
        public System.Windows.Input.ICommand CreateVoucherCommand { get; set; }
        public AddVoucherVM() {
            newCoupon = null;
            Status = new List<string>() { "Pending", "Active"};
            _shopContext = new MyShopContext();

            CreateVoucherCommand = new RelayCommand(CreateVoucher);
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
    }
}
