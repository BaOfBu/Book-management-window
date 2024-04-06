using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Flora.ViewModel
{
    internal class ReportVM : Utilities.ViewModelBase, INotifyPropertyChanged
    {
        private MyShopContext _shopContext;

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
                    UpdateVisiblePeriod();
                    OnPropertyChanged(nameof(SelectedYear));
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
                    UpdateVisiblePeriod();
                    OnPropertyChanged(nameof(SelectedMonth));
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
                    UpdateVisiblePeriod();
                    OnPropertyChanged(nameof(SelectedWeek));
                }
            }
        }

        private SeriesCollection _chartSeries = new SeriesCollection();
        public SeriesCollection ChartSeries
        {
            get { return _chartSeries; }
            set
            {
                _chartSeries = value;
                OnPropertyChanged(nameof(ChartSeries));
            }
        }

        private ObservableCollection<string> _orderDateLabels = new ObservableCollection<string>();
        public ObservableCollection<string> OrderDateLabels
        {
            get => _orderDateLabels;
            set
            {
                _orderDateLabels = value;
                OnPropertyChanged(nameof(OrderDateLabels));
            }
        }
        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                if (_totalRevenue != value)
                {
                    _totalRevenue = value;
                    OnPropertyChanged(nameof(TotalRevenue));
                }
            }
        }

        private int _labelDisplay;
        public int LabelDisplay
        {

            get => _labelDisplay;
            set
            {
                if (_labelDisplay != value)
                {
                    _labelDisplay = value;
                    OnPropertyChanged(nameof(LabelDisplay));
                }
            }
        }

        // Properties for date range selection
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    //UpdateChartSeries();
                    //LoadProductsSales();
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                    //UpdateChartSeries();
                    //LoadProductsSales();
                }
            }
        }


        private void UpdateVisiblePeriod()
        {
            int month;

            if (_selectedMonth.Equals("All month", StringComparison.OrdinalIgnoreCase))
            {
                StartDate = new DateTime(_selectedYear, 1, 1);
                EndDate = new DateTime(_selectedYear, 12, 31);

                UpdateChartSeries();
                LoadProductsSales();
                return;
            }
            else if (int.TryParse(_selectedMonth, out month) && month >= 1 && month <= 12)
            {
                StartDate = new DateTime(_selectedYear, month, 1);
                EndDate = new DateTime(_selectedYear, month, DateTime.DaysInMonth(_selectedYear, month));
            }

            if (_selectedWeek.Equals("All weeks", StringComparison.OrdinalIgnoreCase))
            {
                // Handle case when all weeks are selected
            }
            else if (int.TryParse(_selectedWeek, out int week) && week >= 1 && week <= 4)
            {
                month = int.Parse(_selectedMonth);
                int daysInMonth = DateTime.DaysInMonth(_selectedYear, month);
                int daysInEachWeek = (int)Math.Ceiling((double)daysInMonth / 4);
                int startDay = ((week - 1) * daysInEachWeek) + 1;
                int endDay = Math.Min(week * daysInEachWeek, daysInMonth);

                StartDate = new DateTime(_selectedYear, month, startDay);
                EndDate = new DateTime(_selectedYear, month, endDay);
            }

            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(EndDate));
            UpdateChartSeries();
            LoadProductsSales();
        }

        public void UpdateChartSeries()
        {
            if (PlantsProducts != null && PlantsProducts.Count > 0)
            {
                ChartSeries.Clear();
            }
            _shopContext = new MyShopContext();
            var selectionStartDateOnly = DateOnly.FromDateTime(StartDate);
            var selectionEndDateOnly = DateOnly.FromDateTime(EndDate);
            Debug.WriteLine(selectionStartDateOnly + "()" + selectionEndDateOnly);
            var aggregatedData = _shopContext.Orders
                .Where(o => o.OrderDate.HasValue &&
                            o.OrderDate.Value >= selectionStartDateOnly &&
                            o.OrderDate.Value <= selectionEndDateOnly)
                .ToList()
                .GroupBy(o => o.OrderDate.Value)
                .Select(group => new
                {
                    OrderDate = group.Key,
                    TotalAmount = group.Sum(o => o.TotalAmount ?? 0)
                })
                .OrderBy(result => result.OrderDate)
                .ToList();

            var lineSeries = new LiveCharts.Wpf.LineSeries
            {
                Title = "Sales",
                Values = new ChartValues<decimal>(),
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 15,
                Fill = Brushes.Transparent
            };
            decimal totalRevenue = 0;
            OrderDateLabels.Clear(); // Clear existing labels

            foreach (var item in aggregatedData)
            {
                lineSeries.Values.Add(item.TotalAmount);
                OrderDateLabels.Add(item.OrderDate.ToString("dd/MM"));
                totalRevenue += item.TotalAmount;
            }
            TotalRevenue = totalRevenue;

            // Add the new line series to ChartSeries
            //if (ChartSeries.Count > 0)
            //{
            //    if (aggregatedData.Count > 0)
            //    {

            //        ChartSeries.Clear();
            //    }
            //}


            ChartSeries.Add(lineSeries);

            if (aggregatedData.Count > 0)
            {
                LabelDisplay = 1;
            }
            else
            {
                LabelDisplay = 0;
            }


            OnPropertyChanged(nameof(ChartSeries));
            OnPropertyChanged(nameof(OrderDateLabels));
            OnPropertyChanged(nameof(TotalRevenue));
            OnPropertyChanged(nameof(LabelDisplay));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<object> _plantsProducts;
        public List<object> PlantsProducts
        {
            get { return _plantsProducts; }
            set
            {
                _plantsProducts = value;
                OnPropertyChanged(nameof(PlantsProducts));
            }
        }

        private Func<double, string> _yAxisLabelFormatter;
        public Func<double, string> YAxisLabelFormatter
        {
            get { return _yAxisLabelFormatter; }
            set
            {
                _yAxisLabelFormatter = value;
                OnPropertyChanged(nameof(YAxisLabelFormatter));
            }
        }


        public void LoadProductsSales()
        {
            var selectionStartDateOnly = DateOnly.FromDateTime(StartDate);
            var selectionEndDateOnly = DateOnly.FromDateTime(EndDate);

            // Materialize the query to the client side by calling ToList()
            var productsSales = (from plant in _shopContext.Plants
                                 join orderDetail in _shopContext.OrderDetails on plant.PlantId equals orderDetail.PlantId into odGroup
                                 from od in odGroup.DefaultIfEmpty()
                                 join order in _shopContext.Orders on od.OrderId equals order.OrderId into oGroup
                                 from o in oGroup.DefaultIfEmpty()
                                 where o != null && o.OrderDate >= selectionStartDateOnly && o.OrderDate <= selectionEndDateOnly
                                 group new { plant, od, o } by new { plant.PlantId, plant.Name, plant.Category.CategoryName } into grouped
                                 select new
                                 {
                                     ProductId = grouped.Key.PlantId,
                                     ProductName = grouped.Key.Name,
                                     CategoryName = grouped.Key.CategoryName,
                                     SalesQuantity = grouped.Sum(x => x.od.Quantity ?? 0),
                                     ChartSeries = new SeriesCollection
                                     {
                                         new LineSeries
                                         {
                                             Title = grouped.Key.Name,
                                             Values = new ChartValues<double>(grouped.Select(g => (double)(g.od.Quantity ?? 0)))
                                         }
                                     },
                                     OrderDateLabels = grouped.Select(g => g.o.OrderDate.Value.ToString("dd/MM/yyyy")).ToList(),
                                     LabelDisplay = OrderDateLabels.Count > 0 ? 1 : 0,
                                 })
                                 .ToList<object>();

            if (productsSales.Count == 0)
            {
                if (ChartSeries != null && ChartSeries.Count > 2)
                {
                    ChartSeries.Clear();
                }
            }
            PlantsProducts = productsSales;
        }





        public ReportVM()
        {
            // Constructor or method where you set the formatter
            YAxisLabelFormatter = value => value.ToString("0");

            _shopContext = new MyShopContext();

            AvailableYears = new ObservableCollection<int>();
            for (int year = 2020; year <= DateTime.Now.Year; year++)
            {
                AvailableYears.Add(year);
            }


            AvailableMonths = new ObservableCollection<string>();
            AvailableMonths.Add("All month");
            for (int month = 1; month <= 12; month++)
            {
                AvailableMonths.Add(month.ToString());
            }


            AvailableWeeks = new ObservableCollection<string> { "All weeks", "1", "2", "3", "4" };


            _selectedYear = DateTime.Now.Year;
            _selectedMonth = DateTime.Now.Month.ToString();
            _selectedWeek = "All weeks";
            StartDate = new DateTime(_selectedYear, int.Parse(_selectedMonth), 1);
            EndDate = new DateTime(_selectedYear, int.Parse(_selectedMonth), DateTime.DaysInMonth(_selectedYear, int.Parse(_selectedMonth)));
            UpdateVisiblePeriod();

            LoadProductsSales();

            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(SelectedWeek));
            OnPropertyChanged(nameof(ChartSeries));
        }
    }
}

