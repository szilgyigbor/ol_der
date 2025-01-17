using Ol_der.Data;
using Ol_der.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.DetailedSearch
{
    class SearchWindowRepository
    {
        public async Task<List<Product>> SearchProductsByItemNumberAsync(string itemNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products
                    .Where(p => !p.IsDeleted && p.ItemNumber.Contains(itemNumber))
                    .Include(p => p.Supplier)
                    .ToListAsync();
            }
        }

        public async Task<List<Product>> SearchProductsByNameAsync(string productName)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Products
                    .Where(p => !p.IsDeleted && p.Name.Contains(productName))
                    .Include(p => p.Supplier)
                    .ToListAsync();
            }
        }
    }
}
