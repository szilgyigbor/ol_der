using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ol_der.Controls.Orders
{
    class GreenifyOrderViewModel : INotifyPropertyChanged
    {
        private OrderRepository _orderRepository;
        private Order _order;
        private string _quantity;
        private int _orderId;
        private string _productDescription = "Válassz ki egy terméket a listából";
        private OrderItem _selectedOrderItem;
        private OrderItem _orderItem;

        public Action OnGreened;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
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

        public OrderItem SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                _selectedOrderItem = value;
                Quantity = _selectedOrderItem?.QuantityReceived.ToString();
                ProductDescription = $@"Ennyit rendeltünk:  {SelectedOrderItem?.QuantityOrdered} Ebből: {SelectedOrderItem?.Product.ItemNumber}";
                OnPropertyChanged();
            }
        }

        public ICommand UpdateOrderCommand { get; }
        public ICommand UpdateOrderItemCommand { get; }
        public ICommand FinalizeGreenifyCommand { get; }

        public GreenifyOrderViewModel(int orderId)
        {
            _orderRepository = new OrderRepository();
            _orderId = orderId;
            LoadOrderAsync();
            UpdateOrderCommand = new RelayCommand(param => UpdateOrder());
            UpdateOrderItemCommand = new RelayCommand(param => UpdateOrderItem());
            FinalizeGreenifyCommand = new RelayCommand(param => FinalizeGreenify());
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadOrderAsync()
        {
            Order = await _orderRepository.GetOrderByOrderIdAsync(_orderId);
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

            return true;
        }

        public async Task UpdateOrderItem()
        {
            if (SelectedOrderItem == null)
            {
                MessageBoxOkWindow messageBoxWindow0 = new("Előbb válassz ki egy tételt!");
                messageBoxWindow0.ShowDialog();
                return;
            }

            if (CheckQuantity() == false)
            {
                return;
            }

            SelectedOrderItem.QuantityReceived = int.Parse(Quantity);
            await _orderRepository.UpdateOrderItemAsync(SelectedOrderItem);

            await LoadOrderAsync();

            ProductDescription = "Válassz ki egy terméket a listából";
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

            await LoadOrderAsync();

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen frissítve!");
            messageBoxOkWindow.ShowDialog();
        }


        public async Task FinalizeGreenify()
        {
            try
            {
                MessageBoxWindow messageBoxWindow = new("Biztosan véglegesíted a zöldítést?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult != true)
                {
                    return;
                }

                Order orderToAppend = await _orderRepository.GetLastOpenOrderBySupplierIdAsync(Order.SupplierId);

                if (orderToAppend == null)
                {
                    MessageBoxOkWindow messageBoxOkWindow = new("Nincs nyitott rendelés, újat kezdtünk!");
                    messageBoxOkWindow.ShowDialog();

                    orderToAppend = new Order
                    {
                        SupplierId = _order.SupplierId,
                        OrderDate = DateTime.Now,
                        OrderItems = new List<OrderItem>()
                    };

                    await _orderRepository.AddOrderAsync(orderToAppend);
                }

                await AppendOrderItems(orderToAppend, Order);

                _order.IsColored = true;
                _order.ReOrdered = true;

                Debug.WriteLine("Updating order to append...");
                
                Debug.WriteLine("Order to append updated.");

                Debug.WriteLine("Updating current order...");
                await _orderRepository.UpdateOrderAsync(_order);
                Debug.WriteLine("Current order updated.");

                MessageBoxOkWindow messageBoxOkWindow1 = new("Sikeresen zöldítve!");
                Order = new Order();
                messageBoxOkWindow1.ShowDialog();
                OnGreened?.Invoke();
            }
            catch (InvalidOperationException ex)
            {
                MessageBoxOkWindow errorMessageBox = new($"Hiba történt: {ex.Message}");
                errorMessageBox.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxOkWindow errorMessageBox = new($"Hiba történt: {ex.Message}");
                errorMessageBox.ShowDialog();
            }
        }

        public async Task AppendOrderItems(Order orderToAppend, Order closedOrder)
        {
            foreach (var item in closedOrder.OrderItems)
            {
                var missingQuantity = item.QuantityOrdered - item.QuantityReceived;
                if (missingQuantity > 0)
                {
                    var existingItem = orderToAppend.OrderItems.FirstOrDefault(oi => oi.ProductId == item.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.QuantityOrdered += missingQuantity;
                        await _orderRepository.UpdateOrderItemAsync(existingItem);
                    }
                    else
                    {
                        OrderItem newItem = new OrderItem
                        {
                            OrderId = orderToAppend.OrderId,
                            ProductId = item.ProductId,
                            Product = item.Product,
                            QuantityOrdered = missingQuantity,
                            QuantityReceived = 0,
                            Comment = item.Comment
                        };

                        await _orderRepository.AddOrderItemAsync(newItem);

                    }
                }
            }
        }

    }
}
