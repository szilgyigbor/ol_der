using Ol_der.Controls.SalePackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ol_der.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ol_der.Controls.Orders
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private OrderRepository _orderRepository;

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public OrderViewModel()
        {
            _orderRepository = new OrderRepository();

            Orders = new ObservableCollection<Order>
        {
            new Order { OrderId = 1, Supplier = new Supplier { Name = "Supplier 1" }, OrderDate = DateTime.Now, Comment = "First order" },
            new Order { OrderId = 2, Supplier = new Supplier { Name = "Supplier 2" }, OrderDate = DateTime.Now, Comment = "Second order" }
        };
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            return await _orderRepository.GetAllSupplierAsync();
        }

        public async Task LoadOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrderAsync();
            Orders = new ObservableCollection<Order>(orders);
        }

        public async Task AddOrderAsync(Order newOrder)
        {
            await _orderRepository.AddOrderAsync(newOrder);
            await LoadOrdersAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await _orderRepository.DeleteOrderAsync(order);
            await LoadOrdersAsync();
        }

        public async Task<Supplier> GetSupplierById(int supplierId)
        {
            return await _orderRepository.GetSupplierById(supplierId);
        }

    }

}
