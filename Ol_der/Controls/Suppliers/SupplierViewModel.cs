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

        public void RemoveSupplier(int SupplierId) 
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.SupplierId == SupplierId);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public void ModifySupplier(Supplier modifiedSupplier)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.SupplierId == modifiedSupplier.SupplierId);
            if (supplier != null)
            {
                supplier.Name = modifiedSupplier.Name;
                supplier.Address = modifiedSupplier.Address;
                supplier.Email = modifiedSupplier.Email;
                supplier.Phone = modifiedSupplier.Phone;
                _context.SaveChanges();
            }
        }

        public List<Supplier> GetAllSupplier()
        {
            return _context.Suppliers.ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
