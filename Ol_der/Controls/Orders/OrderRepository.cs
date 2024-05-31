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
                return await context.Suppliers.Where(s => !s.IsDeleted).ToListAsync();
            }
        }

        public async Task<List<Order>> GetAllOrderAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Orders.Include(o => o.Supplier).ToListAsync();
            }
        }

        public async Task AddOrderAsync(Order newOrder)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Orders.Add(newOrder);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(Order order)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Suppliers.FindAsync(supplierId);
            }
        }

        public async Task<Order> GetLastOrderBySupplierIdAsync(int supplierId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Orders
                    .Where(o => o.SupplierId == supplierId && o.IsOpen)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.Supplier)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Supplier)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
