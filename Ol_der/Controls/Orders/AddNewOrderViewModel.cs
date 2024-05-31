using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ol_der.Models;

namespace Ol_der.Controls.Orders
{
    class AddNewOrderViewModel : INotifyPropertyChanged
    {
        private OrderRepository _orderRepository;
        private Order _order;
        private string _title;
        private int _supplierId;

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

        public AddNewOrderViewModel(Order order, int supplierId)
        {
            _orderRepository = new OrderRepository();
            Order = order;
            _supplierId = supplierId;
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
                string message = "Van már nyitott rendelés ehhez a beszállítóhoz!";
                MessageBoxWindow messageBoxWindow = new(message);
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    Title = "Nyitott rendelés módosítása";
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
                Comment = "Valami komi"
            };
            await _orderRepository.AddOrderAsync(newOrder);

            return newOrder;
        }
    }
}
