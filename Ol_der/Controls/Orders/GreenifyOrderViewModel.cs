using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ol_der.Controls.Orders
{
    class GreenifyOrderViewModel : INotifyPropertyChanged
    {
        private OrderRepository _orderRepository;
        private Order _order;
        private string _quantity;
        private string _productDescription = "Válassz ki egy terméket a listából";
        private OrderItem _selectedOrderItem;
        private OrderItem _orderItem;

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
                ProductDescription = $@"Ennyit rendeltünk:  {SelectedOrderItem.QuantityOrdered}
Ebből: {SelectedOrderItem.Product.ItemNumber}";
                OnPropertyChanged();
            }
        }

        public ICommand UpdateOrderCommand { get; }
        public ICommand UpdateOrderItemCommand { get; }
        public ICommand FinalizeGreenifyCommand { get; }

        public GreenifyOrderViewModel(Order order)
        {
            _orderRepository = new OrderRepository();
            _order = order;
            UpdateOrderCommand = new RelayCommand(param => UpdateOrder());
            UpdateOrderItemCommand = new RelayCommand(param => UpdateOrderItem());
            FinalizeGreenifyCommand = new RelayCommand(param => FinalizeGreenify());
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            Order = await _orderRepository.GetOrderByOrderIdAsync(Order.OrderId);

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

            Order = await _orderRepository.GetOrderByOrderIdAsync(Order.OrderId);

            MessageBoxOkWindow messageBoxOkWindow = new("Sikeresen frissítve!");
            messageBoxOkWindow.ShowDialog();
        }


        public async Task FinalizeGreenify()
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
                    SupplierId = Order.SupplierId,
                    OrderDate = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };
            }

            await _orderRepository.AppendMissingItemsToOrderAsync(orderToAppend, Order);

            Order.IsColored = true;
            Order.ReOrdered = true;

            await _orderRepository.UpdateOrderAsync(Order);

            MessageBoxOkWindow messageBoxOkWindow1 = new("Sikeresen zöldítve!");
            messageBoxOkWindow1.ShowDialog();
        }

    }
}
