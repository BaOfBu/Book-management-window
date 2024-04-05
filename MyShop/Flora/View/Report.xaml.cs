using Flora.ViewModel;
using System;
using System.Windows.Controls;

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
            this.Loaded += (s, e) =>
            {
                var viewModel = this.DataContext as ReportVM;
                if (viewModel != null)
                {
                    viewModel.StartDate = new DateTime(viewModel.SelectedYear, 1, 1);
                    viewModel.EndDate = new DateTime(viewModel.SelectedYear, 12, 31);
                }
            };
            ReportVM = DataContext as ReportVM;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }


        private void WeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedValue == "All month")
            {
                WeekComboBox.IsEditable = false;
                WeekComboBox.IsReadOnly = true;
                WeekComboBox.IsEnabled = false;
            }
            else
            {
                if (WeekComboBox != null)
                {
                    WeekComboBox.IsEditable = false;
                    WeekComboBox.IsReadOnly = false;
                    WeekComboBox.IsEnabled = true;
                }

            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
