using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Orders
{
    internal class OrderDetailsViewModel
    {
        private OrderRepository _orderRepository;

        public Order Order { get; set; }

        public OrderDetailsViewModel(int orderId)
        {
            _orderRepository = new OrderRepository();
            LoadOrderAsync(orderId);
        }

        private async Task LoadOrderAsync(int orderId)
        {
            Order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
        }

    }
}
