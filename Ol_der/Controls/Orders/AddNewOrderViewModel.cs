using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public OrderItem OrderItem { get; set; }

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

        public ICommand SearchProductCommand { get; }

        public ICommand SetQuantityCommand { get; }
        public ICommand AddItemCommand { get; }

        public AddNewOrderViewModel(Order order, int supplierId)
        {
            _orderRepository = new OrderRepository();
            Order = order;
            _supplierId = supplierId;
            SearchProductCommand = new RelayCommand(param => SearchProduct());
            SetQuantityCommand = new RelayCommand(param => SetQuantity());
            AddItemCommand = new RelayCommand(param => AddItemToOrder());
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
                string message = "Nincs nyitott rendelés ehhez a beszállítóhoz, újat kezdjünk?";

                MessageBoxWindow messageBoxWindow = new(message);

                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    Title = "Új rendelés kezelése";
                    Order newOrder = await CreateOrderAsync();

                    Order = newOrder;
                }
                else
                {
                    return;
                }
                
            }
            else
            {
                string message = "A kiválasztott (beszállítóhoz tartozó) rendelés módosítása!";
                MessageBoxWindow messageBoxWindow = new(message);
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    Title = "Rendelés módosítása";
                }
                else
                {
                    return;
                }

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
                Comment = "Komment"
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
                    MessageBoxWindow messageBoxWindow = new("Nem ehhez a beszállítóhoz tartozik a termék!");
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
                    Order = Order
                };

                ProductDescription = "";

                ProductDescription += product.ItemNumber + "   " + product.Name;

            }
            else
            {
                MessageBoxWindow messageBoxWindow = new("Nincs ilyen termék!");
                messageBoxWindow.ShowDialog();
            }
        }

        public async Task SetQuantity() 
        {
            if (ItemNumber == null)
            {
                MessageBoxWindow messageBoxWindow = new("Előbb adj hozzá egy terméket");
                messageBoxWindow.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(Quantity))
            {
                MessageBoxWindow messageBoxWindow = new("Nem adtál meg mennyiséget!");
                messageBoxWindow.ShowDialog();
                return;
            }

            if (!int.TryParse(Quantity, out int quantity))
            {
                MessageBoxWindow messageBoxWindow = new("Nem számot adtál meg!");
                messageBoxWindow.ShowDialog();
                return;
            }

            OrderItem.QuantityOrdered = quantity;
        }


        public async Task AddItemToOrder()
        {

            if (OrderItem == null)
            {
                MessageBoxWindow messageBoxWindow1 = new("Előbb adj hozzá egy terméket");
                messageBoxWindow1.ShowDialog();
                return;
            }
            
            MessageBoxWindow messageBoxWindow = new("Biztosan hozzá akarod adni? ");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
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

            MessageBoxWindow messageBoxWindow2 = new("Sikeresen hozzáadva!");
            messageBoxWindow2.ShowDialog();

            Order = await _orderRepository.GetLastOpenOrderBySupplierIdAsync(_supplierId);

            OnPropertyChanged();

        }
    }
}
