using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Customers
{
    public class CustomerRepository
    {
        public async Task AddCustomerAsync(Customer newCustomer)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Customers.Add(newCustomer);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Customers
                    .OrderBy(c => c.Name).ToListAsync();
            }
        }

    }
}
