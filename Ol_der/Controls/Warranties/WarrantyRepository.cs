using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;

namespace Ol_der.Controls.Warranties
{
    public class WarrantyRepository
    {
        public async Task<List<Warranty>> GetLimitedNumberOfWarrantyAsync(int limit)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Warranties
                    .Include(w => w.Product)
                    .Include(w => w.Supplier)
                    .Include(w => w.WarrantyStatuses)
                    .OrderByDescending(w => w.CreationDate)
                    .Take(limit)
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

        public async Task<WarrantyStatus> CreateNewWarrantyStatus(int warrantyId, string content)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var warrantyStatus = new WarrantyStatus
                {
                    WarrantyId = warrantyId,
                    StatusDescription = content,
                    StatusDate = DateTime.Now
                };

                context.WarrantyStatuses.Add(warrantyStatus);
                await context.SaveChangesAsync();

                return warrantyStatus;
            }
        }

        public async Task<Warranty> GetWarrantyByIdAsync(int warrantyId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Warranties
                    .Include(w => w.Product)
                    .Include(w => w.Supplier)
                    .Include(w => w.WarrantyStatuses)
                    .FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);
            }
        }

        public async Task UpdateWarrantyAsync(Warranty warranty)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Warranties.Update(warranty);
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveWarrantyAsync(Warranty warranty)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Warranties.Add(warranty);
                await context.SaveChangesAsync();
            }
        }
    }
}
