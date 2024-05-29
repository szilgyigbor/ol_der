using Ol_der.Controls.SalePackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ol_der.Models;

namespace Ol_der.Controls.Orders
{
    internal class OrderViewModel
    {
        private readonly OrderRepository _orderRepository;

        public OrderViewModel()
        {
            _orderRepository = new OrderRepository();
        }

        /*public List<Order> GetOrders()
        {
            return _orderRepository.GetOrders();

        }*/

        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            return await _orderRepository.GetAllSupplierAsync();
        }
    }
}
