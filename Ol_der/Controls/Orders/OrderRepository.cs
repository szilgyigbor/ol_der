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
                return await context.Orders.OrderByDescending(o => o.OrderDate).Include(o => o.Supplier).ToListAsync();
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

        public async Task<Order> GetLastOpenOrderBySupplierIdAsync(int supplierId)
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

        public async Task<Order> GetLastOpenOrderByOrderIdAsync(int orderId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Orders
                    .Where(o => o.OrderId == orderId && o.IsOpen)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.Supplier)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Supplier)
                    .FirstOrDefaultAsync();
            }
        }


        public async Task<Product> SearchProductByItemNumberAsync(string itemNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products.Where(s => !s.IsDeleted)
                    .FirstOrDefaultAsync(p => p.ItemNumber == itemNumber);
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Orders.Update(order);
                await context.SaveChangesAsync(); 
            }
        }

        public async Task UpdateOrderItemQuantityOrderedAsync(int orderItemId, int quantity)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var orderItem = await context.OrderItems.FindAsync(orderItemId);
                orderItem.QuantityOrdered = quantity;
                await context.SaveChangesAsync();
                
            }
        }

        public async Task RemoveOrderItemAsync(OrderItem orderItem)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.OrderItems.Remove(orderItem);
                await context.SaveChangesAsync();
            }
        }

    }
}
