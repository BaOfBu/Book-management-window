using Flora.ViewModel;
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

            if (RevenueGraph != null && RevenueGraph.AxisY != null && RevenueGraph.AxisY.Count > 0)
            {
                var yAxis = RevenueGraph.AxisY[0];
                yAxis.LabelFormatter = value => value.ToString("0");
            }

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
