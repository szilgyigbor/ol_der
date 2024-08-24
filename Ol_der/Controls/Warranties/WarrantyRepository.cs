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
                try
                {
                    context.Warranties.Update(warranty);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
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

        public async Task RemoveWarrantyStatusAsync(WarrantyStatus warrantyStatus)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                try
                {
                    context.WarrantyStatuses.Remove(warrantyStatus);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task UpdateWarrantyStatusAsync(WarrantyStatus warrantyStatus)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                try
                {
                    context.WarrantyStatuses.Update(warrantyStatus);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task RemoveWarrantyAsync(Warranty warranty)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                try
                {
                    var warrantyStatuses = context.WarrantyStatuses.Where(ws => ws.WarrantyId == warranty.WarrantyId);

                    context.WarrantyStatuses.RemoveRange(warrantyStatuses);

                    context.Warranties.Remove(warranty);

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
