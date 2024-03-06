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

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Vouchers.xaml
    /// </summary>
    public partial class Vouchers : UserControl
    {
        private VoucherVM voucherVM { get; set; }
        public Vouchers()
        {
            InitializeComponent();
            voucherVM = DataContext as VoucherVM;
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
                    if (voucherVM != null)
                    {
                        voucherVM.PageSize = pageSize;
                    }
                }
            }
        }
    }
}
