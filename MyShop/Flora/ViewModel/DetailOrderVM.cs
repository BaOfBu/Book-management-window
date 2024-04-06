using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Flora.Utilities;
using Flora.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.OData.UriParser;
using Telerik.Windows.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Flora.ViewModel
{
    class DetailOrderVM :Utilities.ViewModelBase
    {
        private MyShopContext _shopContext;
        public Order SelectedOrder { get; set; }
        public ObservableCollection<Plant> Plants { get; set; }
        public bool IsEnabledInfo { get; set; }
        public Visibility IsVisible { get; set; }
        public List<Coupon> Coupons { get; set; }
        public Coupon SelectedCoupon { get; set; }
        public int SelectedCouponIndex { get; set; }
        public decimal TotalAmount { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public Order OldOlderData { get; set; }
        public ObservableCollection<Plant> SelectedPlants { get; set; }
        public List<string> Status { get; set; }
        public string SelectedStatus { get; set; }
        public bool IsValidData { get; set; }
        public System.Windows.Input.ICommand AddItemPanelCommand { get; set; }
        public System.Windows.Input.ICommand RemoveItemPanelCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxItemSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxQuantitySelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxVoucherSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand ComboBoxStatusSelectionChangedCommand { get; set; }
        public System.Windows.Input.ICommand RemoveOrderCommand { get; set; }
        public System.Windows.Input.ICommand CreateOrderCommand { get; set; }
        public DetailOrderVM()
        {
        }

        public DetailOrderVM(Order selectedOrder)
        {
            _shopContext = new MyShopContext();
            IsValidData = false;
            SelectedOrder = selectedOrder;
            OldOlderData = (Order)SelectedOrder.Clone();
            LoadStatus();
            SelectedStatus = SelectedOrder.Status;
            SelectedPlants = new ObservableCollection<Plant>();
            LoadPlants();
            LoadCoupons();
            Items = new ObservableCollection<ItemViewModel>();
            IsVisible = Visibility.Visible;
            IsEnabledInfo = true;

            switch (selectedOrder.Status)
            {
                case "Pending":
                {
                    LoadData(true);
                    break;
                }
                case "Delivering":
                {
                    LoadData(true);
                    break;
                }
                case "Delivered":
                {
                    LoadData(false);
                    break;
                }
                case "Cancelled":
                {
                    LoadData(false);
                    break;
                }
                default:
                {
                    LoadData(false);
                    break;
                }
            }

            AddItemPanelCommand = new RelayCommand(AddItemPanel);
            RemoveItemPanelCommand = new RelayCommand(RemoveItemPanel);
            ComboBoxItemSelectionChangedCommand = new RelayCommand(ComboBoxItemSelectionChanged);
            ComboBoxQuantitySelectionChangedCommand = new RelayCommand(ComboBoxQuantitySelectionChanged);
            ComboBoxVoucherSelectionChangedCommand = new RelayCommand(ComboBoxVoucherSelectionChanged);
            ComboBoxStatusSelectionChangedCommand = new RelayCommand(ComboBoxStatusSelectionChanged);
            RemoveOrderCommand = new RelayCommand(RemoveOrder);
            CreateOrderCommand = new RelayCommand(UpdateOrder);

        }
        private IEnumerable<Plant> GetPlantsFromDatabase()
        {
            if (_shopContext.Database.CanConnect())
            {
                return _shopContext.Plants.Include(o => o.Category).Include(o => o.OrderDetails).ThenInclude(od => od.Order).ToList();
            }
            return Enumerable.Empty<Plant>();
        }
        private void LoadData(bool isEnabled)
        {
            IsEnabledInfo = isEnabled;
            int count = 0;
            if(SelectedOrder.OrderDetails != null)
            {
                SelectedPlants = new ObservableCollection<Plant>(SelectedOrder.OrderDetails.Select(od => od.Plant).ToList());
                foreach (var orderDetail in SelectedOrder.OrderDetails)
                {
                    SelectedPlants.Remove(orderDetail.Plant);
                    var array = GetAvailablePlants(Plants);
                    var item = new ItemViewModel()
                    {
                        ItemLabel = "Item " + (count + 1),
                        Plants = array,
                        SelectedPlant = orderDetail.Plant,
                        SelectedPlantIndex = array.ToList().FindIndex(a => a.PlantId == orderDetail.PlantId),
                        IsEnablePlants = isEnabled,
                        IsEnabledQuantityComboBox = isEnabled,
                        ListQuantity = AvailableQuantityForItem(orderDetail.Plant),
                        SelectedQuantity = (int)orderDetail.Quantity,
                        TotalPrice = (decimal)orderDetail.Price,
                    };

                    SelectedPlants.Add(orderDetail.Plant);
                    Items.Add(item);
                    count++;
                }
            }

            SelectedCoupon = SelectedOrder.Coupon;
            if (SelectedCoupon != null)
            {
                SelectedCouponIndex = Coupons.FindIndex(c => c.CouponId == SelectedCoupon.CouponId);
            }
            else
            {
                SelectedCouponIndex = -1;
            }
            if (isEnabled)
            {
                IsVisible = Visibility.Visible;
            }
            else
            {
                IsVisible = Visibility.Collapsed;
            }
            TotalAmount = GetTotalAmount();
        }
        private void LoadPlants()
        {
            Plants = new ObservableCollection<Plant>(GetPlantsFromDatabase().ToList());
            OnPropertyChanged(nameof(Plants));
        }
        private void LoadCoupons()
        {
            Coupons = new List<Coupon>(GetCouponsAvailableFromDatabase().ToList());
            OnPropertyChanged(nameof(Coupons));
        }
        private void LoadStatus()
        {
            Status = new List<string>() { "Pending", "Delivering", "Delivered", "Cancelled" };
            OnPropertyChanged(nameof(Status));
        }
        private IEnumerable<Coupon> GetCouponsAvailableFromDatabase()
        {
            if (_shopContext.Database.CanConnect())
            {
                return _shopContext
                    .Coupons
                    .ToList();
            }
            return Enumerable.Empty<Coupon>();
        }
        private void AddItemPanel(object nextItem)
        {
            Items.Add(new ItemViewModel()
            {
                ItemLabel = "Item " + (Items.Count + 1),
                Plants = GetAvailablePlants(Plants),
                SelectedPlant = null,
                SelectedPlantIndex = -1,
                IsEnablePlants = true,
                IsEnabledQuantityComboBox = false,
                ListQuantity = new List<int>(),
                SelectedQuantity = -1,
                TotalPrice = 0,
            });
        }
        private ObservableCollection<Plant> GetAvailablePlants(ObservableCollection<Plant> plants)
        {
            ObservableCollection<Plant> plantsNotSelected = new ObservableCollection<Plant>(plants);
            foreach (var plant in SelectedPlants)
            {
                var index = plantsNotSelected.ToList().FindIndex(p =>  (p != null) && (plant != null) && (p.PlantId == plant.PlantId));
                if (index != -1)
                {
                    plantsNotSelected.RemoveAt(index);
                }
            }
            return plantsNotSelected;
        }
        private void RemoveItemPanel(object selectedItem)
        {
            var selected = selectedItem as ItemViewModel;
            
            Items.Remove(selected);

            if(selected.SelectedPlant != null)
            {
                for (int i = 0; i < SelectedPlants.Count; i++)
                {
                    if (SelectedPlants[i].PlantId == selected.SelectedPlant.PlantId)
                    {
                        SelectedPlants.RemoveAt(i);
                        break;
                    }
                }
            }
            foreach (var item in Items)
            {
                if (item != selected)
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
            if(selectedItemViewModel != null)
            {
                selectedItemViewModel.IsEnabledQuantityComboBox = true;
            }

            Plant selectedPlant = selectedItemViewModel.SelectedPlant;

            if (selectedPlant != null)
            {
                SelectedPlants.Add(selectedPlant);

                var selectedPlantIds = new HashSet<int>(Items.Where(item => item.SelectedPlant != null)
                                                             .Select(item => item.SelectedPlant.PlantId));
                Plant result = null;
                for (int i = 0; i < SelectedPlants.Count; i++)
                {
                    if (SelectedPlants[i] != null && !selectedPlantIds.Contains(SelectedPlants[i].PlantId))
                    {
                        result = (Plant)SelectedPlants[i].Clone();
                        
                        SelectedPlants.RemoveAt(i);
                        break;
                    }
                }

                foreach (var item in Items)
                {
                    if (item != selectedItemViewModel)
                    {
                        var oldSelectedPlant = item.SelectedPlant;
                        for (int i = 0; i < SelectedPlants.Count; i++)
                        {
                            if (oldSelectedPlant != null && SelectedPlants[i] != null && SelectedPlants[i].PlantId == oldSelectedPlant.PlantId)
                            {
                                SelectedPlants.RemoveAt(i);
                                break;
                            }
                        }
                        if(result != null) item.Plants.Add(result);
                        item.Plants = GetAvailablePlants(item.Plants); 
                        item.SelectedPlantIndex = item.Plants.IndexOf(oldSelectedPlant);
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
        private void ComboBoxStatusSelectionChanged(object selectedStatus)
        {
            SelectedStatus = selectedStatus as string;
        }
        private void RemoveOrder(object order)
        {
            if(SelectedOrder != null)
            {
                var orderDetails = _shopContext.OrderDetails.Where(o => o.OrderId == SelectedOrder.OrderId).ToList();
                _shopContext.OrderDetails.RemoveRange(orderDetails);

                var selectedOrder = _shopContext.Orders.FirstOrDefault(o => o.OrderId == SelectedOrder.OrderId);
                _shopContext.Orders.Remove(selectedOrder);
                _shopContext.SaveChanges();

                SelectedOrder = null;
            }
        }
        private decimal CalculateTotalPriceItem(Plant plant, int quantity)
        {
            if(plant != null)
            {
                return (decimal)(plant.Price * quantity);
            }
            return 0;
        }
        private decimal GetTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var item in Items)
            {
                if(item.SelectedPlant != null && item.SelectedQuantity > 0)
                {
                    decimal totalPrice = item.TotalPrice;
                    totalAmount += totalPrice;
                }
            }

            totalAmount -= SelectedCoupon?.Discount ?? 0;

            if (totalAmount < 0) totalAmount = 0;
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
        private void UpdateOrder(object parameter)
        {
            var customer = parameter as Customer;

            if (!ValidateData(customer))
            {
                return;
            }
            if (SelectedOrder != null)
            {
                OldOlderData = (Order)SelectedOrder.Clone();

                UpdateCustomer(SelectedOrder.Customer);
                SelectedOrder.Customer = customer;

                var orderDetails = new List<OrderDetail>();
                foreach (var item in Items)
                {
                    if(item.SelectedPlant != null && item.SelectedQuantity > 0)
                    {
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = SelectedOrder.OrderId,
                            PlantId = item.SelectedPlant.PlantId,
                            Quantity = item.SelectedQuantity,
                            Price = item.TotalPrice,
                            Order = SelectedOrder,
                            Plant = item.SelectedPlant
                        };
                        orderDetails.Add(orderDetail);
                    }
                }

                int j = 0;
                foreach (var orderDetail in orderDetails)
                {
                    UpdateOrderDetail(SelectedOrder, j, orderDetail);
                    j++;
                }
                SelectedOrder.OrderDetails = orderDetails;

                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    if(i < SelectedOrder.OrderDetails.Count)
                    {
                        var orderDetail = SelectedOrder.OrderDetails.ElementAt(i);

                        item.Plants = GetAvailablePlants(Plants);
                        item.SelectedPlant = orderDetail.Plant;
                        item.SelectedPlantIndex = item.Plants.ToList().FindIndex(a => a.PlantId == orderDetail.PlantId);
                        item.ItemLabel = "Item " + (i + 1);
                        item.IsEnabledQuantityComboBox = true;
                        item.ListQuantity = AvailableQuantityForItem(orderDetail.Plant);
                        item.SelectedQuantity = (int)orderDetail.Quantity;
                        item.TotalPrice = (decimal)orderDetail.Price;
                    }
                    else
                    {
                        Items.RemoveAt(i);
                        i--;
                    }
                    
                }

                SelectedOrder.Status = SelectedStatus;

                SelectedOrder.TotalAmount = GetTotalAmount();
                SelectedOrder.Quantity = GetTotalQuantity();
                OnPropertyChanged(nameof(SelectedOrder));
                SelectedPlants.Clear();
                foreach (var item in Items)
                {
                    SelectedPlants.Add(item.SelectedPlant);
                }
                UpdateOrderDB(SelectedOrder);
                SelectedOrder.Coupon = SelectedCoupon;
            }
        }
        private void UpdateCustomer(Customer customer)
        {
            if (_shopContext.Database.CanConnect())
            {
                var existingOrder = _shopContext.Customers.FirstOrDefault(o => o.CustomerId == customer.CustomerId);

                if (existingOrder != null)
                {
                    existingOrder.Name = customer.Name;
                    existingOrder.Email = customer.Email;
                    existingOrder.Address = customer.Address;
                    existingOrder.Phone = customer.Phone;
                }

                _shopContext.SaveChanges();
            }
        }
        private void UpdateOrderDB(Order order)
        {
            if (_shopContext.Database.CanConnect())
            {
                var existingOrder = _shopContext.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (existingOrder != null)
                {
                    _shopContext.OrderDetails.RemoveRange(existingOrder.OrderDetails);

                    foreach (var orderDetail in SelectedOrder.OrderDetails)
                    {
                        existingOrder.OrderDetails.Add(new OrderDetail
                        {
                            PlantId = orderDetail.PlantId,
                            Quantity = orderDetail.Quantity,
                            Price = orderDetail.Price
                        });
                    }

                    existingOrder.Status = SelectedOrder.Status;
                    existingOrder.TotalAmount = GetTotalAmount();
                    existingOrder.Quantity = GetTotalQuantity();

                    _shopContext.SaveChanges();
                }
            }
        }
        private void UpdateOrderDetail(Order order, int index, OrderDetail orderDetail)
        {
            if (_shopContext.Database.CanConnect())
            {
                if (index < order.OrderDetails.Count)
                {
                    OrderDetail oldOrderDetail = order.OrderDetails.ElementAt(index);
                    if (oldOrderDetail != null)
                    {
                        var existingOrder = _shopContext.OrderDetails.FirstOrDefault(o => ((o.OrderId == oldOrderDetail.OrderId) && (o.PlantId == oldOrderDetail.PlantId)));
                        if (existingOrder != null)
                        {
                            existingOrder.Price = orderDetail.Price;
                            existingOrder.Quantity = orderDetail.Quantity;

                            if (existingOrder.PlantId != orderDetail.PlantId)
                            {
                                _shopContext.OrderDetails.Remove(existingOrder);
                                _shopContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    OrderDetail newOrderDetail = new OrderDetail() { 
                        OrderId = orderDetail.OrderId,
                        PlantId = orderDetail.PlantId,
                        Quantity = orderDetail.Quantity,
                        Price = orderDetail.Price,
                    };
                    _shopContext.OrderDetails.Add(newOrderDetail);
                }
                _shopContext.SaveChanges();
            }
        }

        private bool ValidateData(Customer customer)
        {
            if(!string.IsNullOrEmpty(customer.Email) &&
               customer.Email.Length <= 100 &&
               EmailRule.IsValidEmail(customer.Email) &&
               !string.IsNullOrEmpty(customer.Phone) &&
               PhoneNumberRule.IsValidPhoneNumber(customer.Phone) &&
               !string.IsNullOrEmpty(customer.Address) &&
               AddressRule.IsValidAddress(customer.Address) &&
               !string.IsNullOrEmpty(customer.Name) &&
               FullNameRule.IsValidFullName(customer.Name))
            {
                IsValidData = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Update failed!. Please fill in correct information !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsValidData = false;
                return false;
            }

            if (IsValidData)
            {
                if (SelectedPlants == null || SelectedPlants.Count == 0 || Items.Where(item => item.SelectedPlant != null && item.SelectedQuantity <= 0).ToList().Count > 0)
                {
                    System.Windows.MessageBox.Show("Please choose at least a plant item !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    IsValidData = false;
                    return false;
                }
                return true;
            }

            return false;
        }
    }
}
