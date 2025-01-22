using Ol_der.Data;
using Ol_der.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Products
{
    class ProductRepository
    {
        public async Task<bool> AddProductAsync(Product product)
        {
            if ((await SearchProductByItemNumberAsync(product.ItemNumber)).Count != 0)
            {
                return false;
            }

            using (var context = ApplicationDbContextFactory.Create())
            {
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task UpdateProductAsync(Product product)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Products.Update(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> SearchProductByItemNumberAsync(string itemNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products
                                    .Where(s => !s.IsDeleted)
                                    .Include(p => p.Supplier)
                                    .Where(p => p.ItemNumber == itemNumber)
                                    .ToListAsync();
            }
        }

        public async Task DeleteProductAsync(Product product)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var productInDb = await context.Products.FindAsync(product.ProductId);
                if (productInDb != null)
                {
                    productInDb.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products
                                    .Where(p => !p.IsDeleted)
                                    .Include(p => p.Supplier)
                                    .OrderByDescending(p => p.ProductId)
                                    .ToListAsync();
            }
        }

        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Suppliers
                                    .Where(s => !s.IsDeleted)
                                    .ToListAsync();
            }
        }

        public async Task<int> GetProductCountAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products
                                    .Where(p => !p.IsDeleted)
                                    .CountAsync();
            }
        }

    }
}
