using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Products
{
    internal class ProductViewModel : INotifyPropertyChanged
    {
        private int _productCount;
        public int ProductCount
        {
            get => _productCount;
            set
            {
                if (_productCount != value)
                {
                    _productCount = value;
                    OnPropertyChanged(nameof(ProductCount));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool AddProduct(Product product)
        {
            if (SearchProductByItemNumber(product.ItemNumber).Count != 0) 
            {
                return false;
            }

            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            return true;
        }

        public void UpdateProduct(Product product)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Products.Update(product);
                context.SaveChanges();
            }
        }

        public List<Product> SearchProductByItemNumber(string itemNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var upperitemNumber = itemNumber.ToUpper();
                return context.Products.Include(p => p.Supplier)
                              .Where(p => p.ItemNumber.ToUpper().Contains(upperitemNumber))
                              .ToList();
            }
        }

        public void DeleteProduct(Product product)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }

        public List<Product> GetAllProduct()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Products.Include(p => p.Supplier).ToList();
            }
        }

        public List<Supplier> GetAllSupplier()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Suppliers.ToList();
            }
        }

        public int GetProductCount()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Products.Count();
            }
        }
    }
}
