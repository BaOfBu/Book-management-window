using Flora.ViewModel;
using Microsoft.VisualBasic.Logging;
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
    /// Interaction logic for SearchBoxControl.xaml
    /// </summary>
    public partial class SearchBoxControl : UserControl
    {
        public SearchBoxControl()
        {
            InitializeComponent();
        }

        private void SearchHandle()
        {
            string keyword = txtSearchOrders.Text.Trim();

            if (this.DataContext is OrderVM)
            {
                ((OrderVM)this.DataContext).SearchText = keyword;
            }else if (this.DataContext is VoucherVM)
            {
                ((VoucherVM)this.DataContext).SearchText = keyword;
            }
        }

        private void txtSearchOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchHandle();
            }
        }
    }
}
