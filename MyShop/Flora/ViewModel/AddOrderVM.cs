using Flora.Model;
using Flora.Utilities;
using Flora.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.UriParser;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Telerik.Windows.Data;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Flora.ViewModel
{
    class AddOrderVM : Utilities.ViewModelBase
    {
        private readonly MyShopContext _shopContext;
        public ObservableCollection<Plant> Plants { get; set; }
        public List<Coupon> Coupons { get; set; }
        public Coupon SelectedCoupon { get; set; }
        public decimal TotalAmount { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public Order NewOrder { get; set; }
        public DateOnly OrderDate { get; set; }
        public ObservableCollection<Plant> SelectedPlants { get; set; } 
        public System.Windows.Input.ICommand AddItemPanelCommand { get; set; }
        public System.Windows.Input.ICommand RemoveItemPanelCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxItemSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxQuantitySelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxVoucherSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ClearRadButtonCommand { get; set; }
        public System.Windows.Input.ICommand CreateOrderCommand { get; set; }

        public AddOrderVM()
        {
            _shopContext = new MyShopContext();
            OrderDate = DateOnly.FromDateTime(DateTime.Today);
            LoadPlants();
            LoadCoupons();
            SelectedPlants = new ObservableCollection<Plant>();
            Items = new ObservableCollection<ItemViewModel>();
            AddItemPanel(null);

            AddItemPanelCommand = new RelayCommand(AddItemPanel);
            RemoveItemPanelCommand = new RelayCommand(RemoveItemPanel);
            ComboBoxItemSelectionChangedCommand = new RelayCommand(ComboBoxItemSelectionChanged);
            ComboBoxQuantitySelectionChangedCommand = new RelayCommand(ComboBoxQuantitySelectionChanged);
            ComboBoxVoucherSelectionChangedCommand = new RelayCommand(ComboBoxVoucherSelectionChanged);
            ClearRadButtonCommand = new RelayCommand(ClearRadButton);
            CreateOrderCommand = new RelayCommand(CreateOrder);
        }
        private IEnumerable<Plant> GetPlantsFromDatabase()
        {
            if (_shopContext.Database.CanConnect())
            {
                return _shopContext.Plants.Include(o => o.OrderDetails).ToList();
            }
            return Enumerable.Empty<Plant>();
        }
        private void LoadPlants()
        {
            Plants = new ObservableCollection<Plant> ( GetPlantsFromDatabase().ToList() );
            OnPropertyChanged(nameof(Plants));
        }
        private void LoadCoupons()
        {
            Coupons = new List<Coupon>(GetCouponsAvailableFromDatabase().ToList());
            OnPropertyChanged(nameof(Coupons));
        }
        private IEnumerable<Coupon> GetCouponsAvailableFromDatabase()
        {
            if (_shopContext.Database.CanConnect())
            {
                return _shopContext
                    .Coupons
                    .Where(o => 
                           DateOnly.FromDateTime(DateTime.Now) >= o.StartDate 
                           && DateOnly.FromDateTime(DateTime.Now) <= o.ExpiryDate
                           )
                    .ToList();
            }
            return Enumerable.Empty<Coupon>();
        }
        private void AddItemPanel(object nextItem)
        {
            Items.Add(new ItemViewModel()
            {
                 ItemLabel = "Item " + (Items.Count + 1),
                 Plants = GetAvailablePlants(),
                 SelectedPlant = null,
                 SelectedPlantIndex = -1,
                 IsEnabledQuantityComboBox = false,
                 ListQuantity = new List<int>(),
                 TotalPrice = 0,
            });
        }
        private ObservableCollection<Plant> GetAvailablePlants()
        {
            ObservableCollection<Plant> plantsNotSelected = new ObservableCollection<Plant>(Plants.Except(SelectedPlants).ToList());
            return plantsNotSelected;
        }
        private void RemoveItemPanel(object selectedItem)
        {
            var selected = selectedItem as ItemViewModel;
            Items.Remove(selected);
            SelectedPlants.Remove(selected.SelectedPlant);

            foreach (var item in Items)
            {
                if(item != selected)
                {
                    item.ItemLabel = "Item " + (Items.IndexOf(item) + 1);
                    var oldPlants = item.Plants;

                    oldPlants.Add(selected.SelectedPlant);
                    item.Plants = oldPlants;
                    OnPropertyChanged(nameof(Plant));
                }
            }

            TotalAmount = GetTotalAmount();
        }
        private void ComboBoxItemSelectionChanged(object selectedItem)
        {
            ItemViewModel selectedItemViewModel = selectedItem as ItemViewModel;
            selectedItemViewModel.IsEnabledQuantityComboBox = true;

            Plant selectedPlant = selectedItemViewModel.SelectedPlant;

            if (selectedPlant != null)
            {
                SelectedPlants.Add(selectedPlant);

                foreach(var item in Items)
                {
                    if(item != selectedItemViewModel)
                    {
                        var oldPlants = item.Plants;
                        var oldSelectedPlant = item.SelectedPlant;

                        SelectedPlants.Remove(oldSelectedPlant);
                        item.Plants = new ObservableCollection<Plant>(oldPlants.Except(SelectedPlants).ToList());
                        item.SelectedPlant = oldSelectedPlant;
                        SelectedPlants.Add(oldSelectedPlant);
                    }
                }

                selectedItemViewModel.SelectedPlant = selectedPlant;
                selectedItemViewModel.ListQuantity = AvailableQuantityForItem(selectedPlant);
            }

            if(selectedItemViewModel.IsEnabledQuantityComboBox == true && selectedItemViewModel.SelectedQuantity != -1)
            {
                selectedItemViewModel.TotalPrice = CalculateTotalPriceItem(selectedItemViewModel.SelectedPlant, selectedItemViewModel.SelectedQuantity);
                TotalAmount = GetTotalAmount();
            }
        }
        private List<int> AvailableQuantityForItem(Plant plant)
        {
            return Enumerable.Range(1, (int)plant.StockQuantity).ToList();
        }
        private void ComboBoxQuantitySelectionChanged(object selectedQuantity)
        {
            ItemViewModel selectedItem = selectedQuantity as ItemViewModel;
            int quantity = selectedItem.SelectedQuantity;
            selectedItem.TotalPrice = CalculateTotalPriceItem(selectedItem.SelectedPlant, quantity);
            TotalAmount = GetTotalAmount();
        }
        private void ComboBoxVoucherSelectionChanged(object selectedCoupon)
        {
            SelectedCoupon = selectedCoupon as Coupon;
            TotalAmount = GetTotalAmount();
        }
        private void ClearRadButton(object order)
        {
            Items.Clear();
            AddItemPanel(null);
            TotalAmount = 0;
        }
        private decimal CalculateTotalPriceItem(Plant plant, int quantity)
        {
            return (decimal)(plant.Price * quantity);
        }
        private decimal GetTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var item in Items)
            {
                decimal totalPrice = item.TotalPrice;
                totalAmount += totalPrice;
            }

            totalAmount -= SelectedCoupon?.Discount ?? 0;
            return totalAmount;
        }
        private int GetTotalQuantity()
        {
            int totalQuantity = 0;
            foreach (var item in Items)
            {
                int quantity = item.SelectedQuantity;
                totalQuantity += quantity;
            }
            
            return totalQuantity;
        }
        private void CreateOrder(object parameter)
        {
            var customer = parameter as Customer;

            int customerId = InsertCustomer(customer);

            NewOrder = new Order()
            {
                CustomerId = customerId,
                Quantity = GetTotalQuantity(),
                TotalAmount = TotalAmount,
                OrderDate = DateOnly.FromDateTime(DateTime.Today),
                CouponId = SelectedCoupon.CouponId,
                Status = "Pending",
                Coupon = SelectedCoupon,
                Customer = customer,
            };

            int orderId = InsertOrder(NewOrder);
            
            var orderDetails = new List<OrderDetail>();
            foreach (var item in Items)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = orderId,
                    PlantId = item.SelectedPlant.PlantId,
                    Quantity = item.SelectedQuantity,
                    Price = item.TotalPrice,
                    Order = NewOrder,
                    Plant = item.SelectedPlant
                };
                orderDetails.Add(orderDetail);
                InsertOrderDetail(orderDetail);
            }
            NewOrder.OrderDetails = orderDetails;
        }
        private int InsertCustomer(Customer customer)
        {
            if (_shopContext.Database.CanConnect())
            {
                _shopContext.Customers.Add(customer);
                _shopContext.SaveChanges();
            }

            var id = customer.CustomerId;
            return id;
        }
        private int InsertOrder(Order order)
        {
            if (_shopContext.Database.CanConnect())
            {
                _shopContext.Orders.Add(order);
                _shopContext.SaveChanges();
            }

            var id = order.OrderId;
            return id;
        }
        private void InsertOrderDetail(OrderDetail orderDetail)
        {
            if (_shopContext.Database.CanConnect())
            {
                _shopContext.OrderDetails.Add(orderDetail);
                _shopContext.SaveChanges();
            }
        }
    }
}
