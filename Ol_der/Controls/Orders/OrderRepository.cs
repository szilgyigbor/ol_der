using Ol_der.Data;
using Ol_der.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Orders
{
    public class OrderRepository
    {
        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Suppliers.ToListAsync();
            }
        }

        public async Task<List<Order>> GetAllOrderAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Orders.Include(o => o.Supplier).ToListAsync();
            }
        }
    }
}
