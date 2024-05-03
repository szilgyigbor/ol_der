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
        public bool AddSupplier(Supplier newSupplier)
        {
            if (SearchSupplierByName(newSupplier.Name))
            {
                return false;
            }

            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Suppliers.Add(newSupplier);
                context.SaveChanges();
            }

            return true;
        }

        public bool SearchSupplierByName(string name)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Suppliers.Any(s => s.Name == name);
            }
        }

        public void RemoveSupplier(int SupplierId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var supplier = context.Suppliers.FirstOrDefault(s => s.SupplierId == SupplierId);
                if (supplier != null)
                {
                    context.Suppliers.Remove(supplier);
                    context.SaveChanges();
                }
            }
        }

        public void ModifySupplier(Supplier modifiedSupplier)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var supplier = context.Suppliers.FirstOrDefault(s => s.SupplierId == modifiedSupplier.SupplierId);
                if (supplier != null)
                {
                    supplier.Name = modifiedSupplier.Name;
                    supplier.Address = modifiedSupplier.Address;
                    supplier.Email = modifiedSupplier.Email;
                    supplier.Phone = modifiedSupplier.Phone;
                    context.SaveChanges();
                }
            }
        }

        public List<Supplier> GetAllSupplier()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Suppliers.ToList();
            }
        }
    }
}
