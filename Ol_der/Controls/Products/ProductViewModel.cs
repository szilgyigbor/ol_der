using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Products
{
    internal class ProductViewModel
    {
        private ApplicationDbContext _context;

        public ProductViewModel()
        {
            _context = ApplicationDbContextFactory.Create();
        }

        private void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        private void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        private void DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        private List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
