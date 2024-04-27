using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<Sale> GetAllSale()
        {
            return _context.Sales
                   .Include(s => s.SaleItems)
                       .ThenInclude(si => si.Product)
                   .Include(s => s.SaleItems)
                       .ThenInclude(si => si.Product.Supplier)
                   .ToList();
        }

        public Product SearchProductByItemNumber(string itemNumber)
        {
            var upperitemNumber = itemNumber.ToUpper();
            return _context.Products
                           .FirstOrDefault(p => p.ItemNumber.ToUpper().Contains(upperitemNumber));
        }

        public void DeleteSale(Sale sale)
        {
            _context.Sales.Remove(sale);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
