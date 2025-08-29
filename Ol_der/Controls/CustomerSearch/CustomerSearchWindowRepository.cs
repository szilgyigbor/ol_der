using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.CustomerSearch
{
    internal class CustomerSearchWindowRepository
    {
        public async Task<List<Customer>> SearchCustomersByNameAsync(string customerName)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Customers
                    .Where(c => c.Name.Contains(customerName))
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Customers
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
        }
    }
}
