﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Ol_der.Models;

namespace Ol_der.Controls.Orders
{
    class AddNewOrderViewModel : INotifyPropertyChanged
    {
        private OrderRepository _orderRepository;
        private Order _order;
        private string _itemNumber;
        private string _title;
        private int _supplierId;
        private string _productDescription;
        private string _quantity;
        private OrderItem _selectedOrderItem;
        private OrderItem _orderItem;

        

        public event PropertyChangedEventHandler PropertyChanged;
        public Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ItemNumber
        {
            get { return _itemNumber; }
            set
            {
                _itemNumber = value;
                OnPropertyChanged();
            }
        }

        public string ProductDescription
        {
            get { return _productDescription; }
            set
            {
                _productDescription = value;
                OnPropertyChanged();
            }
        }

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public OrderItem OrderItem
        {
            get { return _orderItem; }
            set
            {
                _orderItem = value;
                OnPropertyChanged();
            }
        }

        public OrderItem SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                _selectedOrderItem = value;
                OrderItem = _selectedOrderItem;
                Quantity = _selectedOrderItem?.QuantityOrdered.ToString();
                ProductDescription = "";
                ProductDescription += SelectedOrderItem.Product.ItemNumber + "   " + SelectedOrderItem.Product.Name;
                OnPropertyChanged();
            }
        }


        public ICommand SearchProductCommand { get; }

        public ICommand AddItemCommand { get; }

        public ICommand SaveCommentCommand { get; }

        public ICommand UpdateOrderCommand { get; }

        public ICommand DeleteOrderItemFromOrderCommand { get; }

        public ICommand UpdateOrderItemCommand { get; }

        public ICommand UpdateOrderFromSalesCommand { get; }

        public ICommand CloseAndSaveOrderCommand { get; }

        public AddNewOrderViewModel(Order order, int supplierId)
        {
            _orderRepository = new OrderRepository();
            Order = order;
            _supplierId = supplierId;
            SearchProductCommand = new RelayCommand(param => SearchProduct());
            AddItemCommand = new RelayCommand(param => AddItemToOrder());
            SaveCommentCommand = new RelayCommand(param => SaveComment());
            UpdateOrderCommand = new RelayCommand(param => UpdateOrder());
            DeleteOrderItemFromOrderCommand = new RelayCommand(param => DeleteOrderItemFromOrder());
            UpdateOrderItemCommand = new RelayCommand(param => UpdateOrderItem());
            UpdateOrderFromSalesCommand = new RelayCommand(param => UpdateOrderFromSales());
            CloseAndSaveOrderCommand = new RelayCommand(param => CloseAndSaveOrder());
            CheckOrder();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CheckOrder() 
        {
            if (Order == null)
            {
                string message = "Nincs nyitott rendelés ehhez a beszállítóhoz, újat kezdünk!";
                MessageBoxOkWindow messageBoxWindow = new(message);
                messageBoxWindow.ShowDialog();
                Title = "Új rendelés kezelése";
                Order newOrder = await CreateOrderAsync();
                Order = newOrder;
                
            }
            else
            {
                string message = "A kiválasztott (beszállítóhoz tartozó) rendelés módosítása!";
                MessageBoxOkWindow messageBoxWindow = new(message);
                messageBoxWindow.ShowDialog();
                Title = "Rendelés módosítása";
            }
        }

        public async Task<Order> CreateOrderAsync()
        {

            var newOrder = new Order
            {
                SupplierId = _supplierId,
                OrderDate = DateTime.Now,
                IsOpen = true,
                IsColored = false,
                ReOrdered = false,
            };
            await _orderRepository.AddOrderAsync(newOrder);

            return newOrder;
        }

        public async Task SearchProduct()
        { 
            Product product = await _orderRepository.SearchProductByItemNumberAsync(ItemNumber);

            if (product != null)
            {
                if (product.SupplierId != _supplierId)
                {
                    MessageBoxOkWindow messageBoxWindow = new("Nem ehhez a beszállítóhoz tartozik a termék!");
                    messageBoxWindow.ShowDialog();
                    return;
                }


                OrderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Product = product,
                    QuantityOrdered = 1,
                    QuantityReceived = 0,
                    OrderId = Order.OrderId,
                    Order = Order,
                    Comment = ""
                };

                ProductDescription = "";

                ProductDescription += product.ItemNumber + "   " + product.Name;

            }
            else
            {
                MessageBoxOkWindow messageBoxWindow = new("Nincs ilyen termék!");
                messageBoxWindow.ShowDialog();
            }
        }

        public bool CheckQuantity() 
        {

            if (string.IsNullOrEmpty(Quantity))
            {
                MessageBoxOkWindow messageBoxWindow = new("Nem adtál meg mennyiséget!");
                messageBoxWindow.ShowDialog();
                return false;
            }

            if (!int.TryParse(Quantity, out int quantity))
            {
                MessageBoxOkWindow messageBoxWindow = new("Nem számot adtál meg!");
                messageBoxWindow.ShowDialog();
                return false;
            }

            if (OrderItem != null) 
            {
                OrderItem.QuantityOrdered = quantity;
            }
            
            return true;
        }


        public async Task AddItemToOrder()
        {
            if (OrderItem == null)
            {
                MessageBoxOkWindow messageBoxWindow1 = new("Előbb adj hozzá egy terméket");
                messageBoxWindow1.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new("Biztosan hozzá akarod adni? ");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            if (CheckQuantity() == false)
            {
                return;
            }

            var existingItem = Order.OrderItems.FirstOrDefault(oi => oi.ProductId == OrderItem.ProductId);

            if (existingItem != null)
            {
                existingItem.QuantityOrdered += int.Parse(Quantity);
            }
            else
            {
                Order.OrderItems.Add(OrderItem);
            }

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxWindow2 = new("Sikeresen hozzáadva!");
            messageBoxWindow2.ShowDialog();

            Order = await _orderRepository.GetLastOpenOrderBySupplierIdAsync(_supplierId);
        }

        public async Task SaveComment()
        {
            MessageBoxWindow messageBoxWindow = new("Biztosan el akarod menteni a megjegyzést?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen elmentve!");
            messageBoxOkWindow.ShowDialog();
        }

        public async Task UpdateOrder()
        {
            MessageBoxWindow messageBoxWindow = new("Biztosan rá akarsz menteni a rendelésre?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            Order.OrderDate = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(Order);

            Order = await _orderRepository.GetLastOpenOrderBySupplierIdAsync(_supplierId);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen frissítve!");
            messageBoxOkWindow.ShowDialog();
        }



        public async Task CloseAndSaveOrder()
        {
            MessageBoxWindow messageBoxWindow = new("Biztosan le akarod zárni a rendelést?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            MessageBoxWindow messageBoxWindow1 = new("Tuti biztos?");
            messageBoxWindow1.ShowDialog();

            if (messageBoxWindow1.DialogResult != true)
            {
                return;
            }

            Order.IsOpen = false;
            Order.OrderDate = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen lezárva, új rendelést nyitottunk!");
            messageBoxOkWindow.ShowDialog();

            Order = await CreateOrderAsync();

        }

        public async Task DeleteOrderItemFromOrder()
        {
            if (SelectedOrderItem == null)
            {
                MessageBoxOkWindow messageBoxWindow0 = new("Előbb válassz ki egy tételt!");
                messageBoxWindow0.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new("Biztosan törölni akarod a tételt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            await _orderRepository.RemoveOrderItemAsync(SelectedOrderItem);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen törölve!");
            messageBoxOkWindow.ShowDialog();

            Order = await _orderRepository.GetLastOpenOrderByOrderIdAsync(Order.OrderId);
        }

        public async Task UpdateOrderItem()
        {
            if (SelectedOrderItem == null)
            {
                MessageBoxOkWindow messageBoxWindow0 = new("Előbb válassz ki egy tételt!");
                messageBoxWindow0.ShowDialog();
                return;
            }

            MessageBoxWindow messageBoxWindow = new("Biztosan módosítani akarod a tételt?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            if (CheckQuantity() == false)
            {
                return;
            }

            OrderItem.QuantityOrdered = int.Parse(Quantity);
            await _orderRepository.UpdateOrderItemAsync(OrderItem);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen módosítva!");
            messageBoxOkWindow.ShowDialog();

            Order = await _orderRepository.GetLastOpenOrderByOrderIdAsync(Order.OrderId);
        }

        public async Task UpdateOrderFromSales() 
        {
            MessageBoxWindow messageBoxWindow = new("Biztosan átnézzük az eladásokat és a csomagokat?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            await _orderRepository.UpdateOrderFromSalesForSupplierAsync(Order.SupplierId);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen frissítve!");
            messageBoxOkWindow.ShowDialog();

            Order = await _orderRepository.GetLastOpenOrderByOrderIdAsync(Order.OrderId);
        }
    }
}
