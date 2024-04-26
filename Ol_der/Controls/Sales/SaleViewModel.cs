using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Sales
{
    internal class SaleViewModel : IDisposable
    {
        private ApplicationDbContext _context;

        public SaleViewModel()
        {
            _context = ApplicationDbContextFactory.Create();
        }

        public void AddSale(Sale newSale)
        {
            _context.Sales.Add(newSale);
            _context.SaveChanges();
        }

        public Product SearchProductByItemNumber(string itemNumber)
        {
            var upperitemNumber = itemNumber.ToUpper();
            return _context.Products
                           .FirstOrDefault(p => p.ItemNumber.ToUpper().Contains(upperitemNumber));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
