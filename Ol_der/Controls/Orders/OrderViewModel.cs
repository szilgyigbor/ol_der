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

        public int limitToShow;


        public OrderViewModel(int limit = 100)
        {
            _orderRepository = new OrderRepository();
            limitToShow = limit;
            LoadOrdersAsync(limitToShow);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            return await _orderRepository.GetAllSupplierAsync();
        }

        public async Task LoadOrdersAsync(int limit)
        {
            var orders = await _orderRepository.GetAllOrderAsync(limit);
            Orders = new ObservableCollection<Order>(orders);
        }

        public async Task AddOrderAsync(Order newOrder)
        {
            await _orderRepository.AddOrderAsync(newOrder);
            await LoadOrdersAsync(limitToShow);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await _orderRepository.DeleteOrderAsync(order);
            await LoadOrdersAsync(limitToShow);
        }

        public async Task<Supplier> GetSupplierById(int supplierId)
        {
            return await _orderRepository.GetSupplierByIdAsync(supplierId);
        }


        public async Task<Order> GetLastOpenOrderBySupplierIdAsync(int supplierId)
        {
            Order OrderToModify = await _orderRepository.GetLastOpenOrderBySupplierIdAsync(supplierId);

            return OrderToModify;
        }

        public async Task<Order> GetLastOpenOrderByOrderIdAsync(int orderId)
        {
            Order OrderToModify = await _orderRepository.GetLastOpenOrderByOrderIdAsync(orderId);

            return OrderToModify;
        }

        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            Order OrderToGreenify = await _orderRepository.GetOrderByOrderIdAsync(orderId);

            return OrderToGreenify;
        }
    }

}
