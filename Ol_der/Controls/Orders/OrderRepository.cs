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

        public async Task RemoveOrderItemAsync(OrderItem orderItem)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.OrderItems.Remove(orderItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderFromSalesForSupplierAsync(int supplierId, int monthsToLookBack = 1)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var lookBackDate = DateTime.Now.AddMonths(-monthsToLookBack);

                var saleItems = await context.SaleItems
                    .Where(si => si.Product.SupplierId == supplierId && !si.IsOrdered && si.Product.Supplier.IsDeleted == false)
                    .Include(si => si.Product)
                    .ThenInclude(p => p.Supplier)
                    .Include(si => si.Product)
                    .Where(si => si.Product.SupplierId == supplierId && si.Sale.Date >= lookBackDate && si.Sale.Date <= DateTime.Now)
                    .ToListAsync();

                if (!saleItems.Any())
                {
                    return;
                }

                var openOrder = await context.Orders
                    .Where(o => o.SupplierId == supplierId && o.IsOpen)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync();

                if (openOrder == null)
                {
                    openOrder = new Order
                    {
                        SupplierId = supplierId,
                        OrderDate = DateTime.Now,
                        IsOpen = true,
                        OrderItems = new List<OrderItem>()
                    };
                    context.Orders.Add(openOrder);
                }

                foreach (var saleItem in saleItems)
                {
                    var existingOrderItem = openOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == saleItem.ProductId);

                    if (existingOrderItem != null)
                    {
                        existingOrderItem.QuantityOrdered += saleItem.Quantity;
                    }
                    else
                    {
                        openOrder.OrderItems.Add(new OrderItem
                        {
                            ProductId = saleItem.ProductId,
                            QuantityOrdered = saleItem.Quantity,
                            QuantityReceived = 0
                        });
                    }

                    saleItem.IsOrdered = true;
                }

                await context.SaveChangesAsync();
            }
        }


        public async Task UpdateOrderItemAsync(OrderItem orderItem) {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.OrderItems.Update(orderItem);
                await context.SaveChangesAsync();
            }
        }
        

        public async Task CloseOrder(Order order)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                order.IsOpen = false;
                context.Orders.Update(order);
                await context.SaveChangesAsync();
            }
        }
    }
}
