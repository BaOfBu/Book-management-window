using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Flora.ViewModel
{
    internal class ReportVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private MyShopContext _shopContext;
        private DateTime _periodStart;
        public DateTime PeriodStart
        {
            get => _periodStart;
            set
            {
                if (_periodStart != value)
                {
                    _periodStart = value;
                    OnPropertyChanged(nameof(PeriodStart));
                }
            }
        }

        private DateTime _periodEnd;
        public DateTime PeriodEnd
        {
            get => _periodEnd;
            set
            {
                if (_periodEnd != value)
                {
                    _periodEnd = value;
                    OnPropertyChanged(nameof(PeriodEnd));
                }
            }
        }

        private DateTime _selectionStart;
        public DateTime SelectionStart
        {
            get => _selectionStart;
            set
            {
                if (_selectionStart != value)
                {

                    _selectionStart = value;
                    OnPropertyChanged(nameof(SelectionStart));
                }
            }
        }

        private DateTime _selectionEnd;
        public DateTime SelectionEnd
        {
            get => _selectionEnd;
            set
            {
                if (_selectionEnd != value)
                {
                    _selectionEnd = value;
                    OnPropertyChanged(nameof(SelectionEnd));
                }
            }
        }


        public ObservableCollection<int> AvailableYears { get; set; }
        public ObservableCollection<string> AvailableMonths { get; set; }
        public ObservableCollection<string> AvailableWeeks { get; set; }

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateVisiblePeriod();

                }
            }
        }

        private string _selectedMonth;
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                    UpdateVisiblePeriod();
                }
            }
        }

        private string _selectedWeek;
        public string SelectedWeek
        {
            get => _selectedWeek;
            set
            {
                if (_selectedWeek != value)
                {
                    _selectedWeek = value;
                    OnPropertyChanged(nameof(SelectedWeek));
                    UpdateVisiblePeriod();
                }
            }
        }


        public ReportVM()
        {
            // Initialize the AvailableYears collection from 2000 to the current year
            AvailableYears = new ObservableCollection<int>();
            for (int year = 2000; year <= DateTime.Now.Year; year++)
            {
                AvailableYears.Add(year);
            }

            // Initialize the AvailableMonths collection from 1 to 12
            AvailableMonths = new ObservableCollection<string>();
            AvailableMonths.Add("All month");
            for (int month = 1; month <= 12; month++)
            {
                AvailableMonths.Add(month.ToString());
            }

            // Initialize the AvailableWeeks collection from 1 to 4
            AvailableWeeks = new ObservableCollection<string> { "All weeks", "1", "2", "3", "4" };

            // Set initial selected values to current date's year and month, and first week
            _selectedYear = DateTime.Now.Year;
            _selectedMonth = DateTime.Now.Month.ToString();
            _selectedWeek = "All weeks";
            UpdateVisiblePeriod();
            // This ensures UI is updated with these initial selections
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(SelectedWeek));
            OnPropertyChanged(nameof(SelectionStart));
            OnPropertyChanged(nameof(SelectionEnd));
            // Call any initialization method if needed to load data based on these selections

        }
        private void UpdateVisiblePeriod()
        {
            Debug.WriteLine($"Month: {_selectedMonth}, Week: {_selectedWeek}");
            int month;

            // Handle "All month" selection
            if (_selectedMonth.Equals("All month", StringComparison.OrdinalIgnoreCase))
            {
                PeriodStart = new DateTime(_selectedYear, 1, 1);
                PeriodEnd = new DateTime(_selectedYear, 12, 31);
            }
            else if (int.TryParse(_selectedMonth, out month) && month >= 1 && month <= 12)
            {
                // Specific month selected
                PeriodStart = new DateTime(_selectedYear, month, 1);
                PeriodEnd = new DateTime(_selectedYear, month, DateTime.DaysInMonth(_selectedYear, month));
            }

            // Update SelectionStart and SelectionEnd based on week selection
            if (_selectedWeek.Equals("All weeks", StringComparison.OrdinalIgnoreCase))
            {
                // If "All weeks" is selected, cover the entire month
                SelectionStart = PeriodStart;
                SelectionEnd = PeriodEnd;
            }
            else if (int.TryParse(_selectedWeek, out int week) && week >= 1 && week <= 4)
            {
                // Specific week within a month
                month = int.Parse(_selectedMonth);
                int daysInMonth = DateTime.DaysInMonth(_selectedYear, month);
                int daysInEachWeek = (int)Math.Ceiling((double)daysInMonth / 4); // Adjusted to handle varying number of days in a month
                int startDay = ((week - 1) * daysInEachWeek) + 1;
                int endDay = Math.Min(week * daysInEachWeek, daysInMonth); // Ensure endDay doesn't exceed days in month

                SelectionStart = new DateTime(_selectedYear, month, startDay);
                SelectionEnd = new DateTime(_selectedYear, month, endDay);
            }
            else
            {
                // Fallback if week parsing fails or out of expected range, cover the entire month
                SelectionStart = PeriodStart;
                SelectionEnd = PeriodEnd;
            }

            Debug.WriteLine($"PeriodStart: {PeriodStart}, PeriodEnd: {PeriodEnd}, SelectionStart: {SelectionStart}, SelectionEnd: {SelectionEnd}");

            // Notify UI about the changes
            OnPropertyChanged(nameof(PeriodStart));
            OnPropertyChanged(nameof(PeriodEnd));
            OnPropertyChanged(nameof(SelectionStart));
            OnPropertyChanged(nameof(SelectionEnd));
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
