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
        private ApplicationDbContext _context;

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

        public ProductViewModel()
        {
            _context = ApplicationDbContextFactory.Create();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public List<Product> SearchProductByItemNumber(string itemNumber)
        {
            var upperitemNumber = itemNumber.ToUpper();
            return _context.Products
                           .Where(p => p.ItemNumber.ToUpper().Contains(upperitemNumber))
                           .ToList();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetAllProduct()
        {
            return _context.Products.Include(p => p.Supplier).ToList();
        }

        public List<Supplier> GetAllSupplier()
        {
            return _context.Suppliers.ToList();
        }

        public int GetProductCount()
        {
            return _context.Products.Count();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
