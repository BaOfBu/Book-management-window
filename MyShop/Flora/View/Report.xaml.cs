using Flora.ViewModel;
using System;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Flora.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        private ReportVM ReportVM { get; set; }
        public Report()
        {
            InitializeComponent();
            ReportVM = DataContext as ReportVM;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TimeBar_SelectionChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var view = DataContext as ReportVM;
            var timeBar = sender as RadTimeBar;
            if (timeBar != null)
            {
                var newSelectionStart = (DateTime)timeBar.SelectionStart;
                var newSelectionEnd = (DateTime)timeBar.SelectionEnd;
                view.SelectionStart = newSelectionStart;
                view.SelectionEnd = newSelectionEnd;
            }
        }
    }
}
