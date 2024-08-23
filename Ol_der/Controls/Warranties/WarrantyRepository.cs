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
    }
}
