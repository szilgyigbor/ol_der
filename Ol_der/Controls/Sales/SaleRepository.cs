using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Sales
{
    class SaleRepository
    {
        public async Task AddSaleAsync(Sale newSale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Add(newSale);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Sale>> GetAllSaleAsync(int limit)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                    .OrderByDescending(s => s.Date)
                    .Where(s => !s.IsPackage)
                    .Take(limit)
                    .ToListAsync();
            }
        }

        public async Task<List<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                    .OrderByDescending(s => s.Date)
                    .Where(s => s.Date.Date >= startDate.Date && s.Date.Date <= endDate.Date && !s.IsPackage)
                    .ToListAsync();
            }
        }

        public async Task<List<Sale>> GetSalesByDateRangeForRiportAsync(DateTime startDate, DateTime endDate)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Sales
                    .Where(s => s.Date.Date >= startDate.Date && s.Date.Date <= endDate.Date && !s.IsPackage)
                    .OrderByDescending(s => s.Date)
                    .ToListAsync();
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

        public async Task DeleteSaleAsync(Sale sale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Remove(sale);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Sale> GetSaleAsync(int saleId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                    .FirstOrDefaultAsync(s => s.SaleId == saleId);
            }
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Update(sale);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveAllSaleItemsFromSaleAsync(int saleId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var sale = await context.Sales
                    .Include(s => s.SaleItems)
                    .FirstOrDefaultAsync(s => s.SaleId == saleId);

                if (sale != null && sale.SaleItems.Any())
                {
                    sale.SaleItems.Clear();
                    context.Sales.Update(sale);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Sale>> GetSalesByItemNumberAsync(string productNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                        .Where(s => s.SaleItems.Any(si => si.Product.ItemNumber == productNumber))
                    .OrderByDescending(s => s.Date)
                    .Where(s => !s.IsPackage)
                    .ToListAsync();
            }
        }

        public async Task<List<Sale>> GetSalesByCriteriaAsync(Dictionary<string, string> criteria)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var query = context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(criteria["ProductNumber"]))
                {
                    query = query.Where(s => s.SaleItems.Any(si => si.Product.ItemNumber == criteria["ProductNumber"]));
                }

                if (!string.IsNullOrWhiteSpace(criteria["CustomerName"]))
                {
                    query = query.Where(s => s.CustomerName.Contains(criteria["CustomerName"]));
                }

                query = query.Where(s => !s.IsPackage)
                             .OrderByDescending(s => s.Date);

                return await query.ToListAsync();
            }
        }

    }
}
