using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.CustomerOrders
{
    public class CustomerOrderRepository
    {
        public async Task<List<CustomerOrder>> GetLimitedNumberOfCustomerOrderAsync(int limit)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.CustomerOrders
                    .Include(c => c.CustomerOrderStatuses)
                    .OrderByDescending(w => w.CreationDate)
                    .Take(limit)
                    .ToListAsync();
            }
        }


    }
}
