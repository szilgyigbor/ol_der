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

        public ICommand AddItemCommand { get; }

        public ICommand SaveCommentCommand { get; }

        public ICommand UpdateOrderCommand { get; }

        public AddNewOrderViewModel(Order order, int supplierId)
        {
            _orderRepository = new OrderRepository();
            Order = order;
            _supplierId = supplierId;
            SearchProductCommand = new RelayCommand(param => SearchProduct());
            AddItemCommand = new RelayCommand(param => AddItemToOrder());
            SaveCommentCommand = new RelayCommand(param => SaveComment());
            UpdateOrderCommand = new RelayCommand(param => UpdateOrder());
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
                string message = "Nincs nyitott rendelés ehhez a beszállítóhoz, újat kezdjünk!";
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
                    Order = Order
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

        public void CheckQuantity() 
        {
            if (ItemNumber == null)
            {
                MessageBoxOkWindow messageBoxWindow = new("Előbb adj hozzá egy terméket");
                messageBoxWindow.ShowDialog();
                return;
            }

            if (string.IsNullOrEmpty(Quantity))
            {
                MessageBoxOkWindow messageBoxWindow = new("Nem adtál meg mennyiséget!");
                messageBoxWindow.ShowDialog();
                return;
            }

            if (!int.TryParse(Quantity, out int quantity))
            {
                MessageBoxOkWindow messageBoxWindow = new("Nem számot adtál meg!");
                messageBoxWindow.ShowDialog();
                return;
            }

            OrderItem.QuantityOrdered = quantity;
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

            CheckQuantity();

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

            OnPropertyChanged();

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
            MessageBoxWindow messageBoxWindow = new("Biztosan el akarod menteni a rendelést?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            Order.OrderDate = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen elmentve!");
            messageBoxOkWindow.ShowDialog();
        }



        public async Task CloseAndSaveOrder()
        {
            MessageBoxWindow messageBoxWindow = new("Biztosan el akarod menteni a rendelést?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            Order.IsOpen = false;
            Order.OrderDate = DateTime.Now;

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen elmentve!");
            messageBoxOkWindow.ShowDialog();
        }
    }
}
