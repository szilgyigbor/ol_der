using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Suppliers
{
    class SupplierViewModel
    {
        private ApplicationDbContext _context;

        public SupplierViewModel()
        {
            _context = ApplicationDbContextFactory.Create();
        }

        public void AddSupplier(Supplier newSupplier)
        {
            _context.Suppliers.Add(newSupplier);
            _context.SaveChanges();
        }

        private List<Supplier> GetAllSupplier()
        {
            return _context.Suppliers.ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
