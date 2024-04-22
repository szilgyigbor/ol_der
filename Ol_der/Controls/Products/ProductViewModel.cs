﻿using Microsoft.EntityFrameworkCore;
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

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}