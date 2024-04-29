﻿using Microsoft.EntityFrameworkCore;
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
    internal class SaleViewModel
    {
        public void AddSale(Sale newSale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Add(newSale);
                context.SaveChanges();
            }
        }

        public List<Sale> GetAllSale()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier)
                    .OrderByDescending(s => s.Date)
                    .ToList();
            }
        }

        public Product SearchProductByItemNumber(string itemNumber)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                var upperItemNumber = itemNumber.ToUpper();
                return context.Products
                    .FirstOrDefault(p => p.ItemNumber.ToUpper().Contains(upperItemNumber));
            }
        }

        public void DeleteSale(Sale sale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Remove(sale);
                context.SaveChanges();
            }
        }

        public Sale GetSale(int saleId)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
               return context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier).FirstOrDefault(s => s.SaleId == saleId);

            }
        }

        public void UpdateSale(Sale sale)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Sales.Update(sale);
                context.SaveChanges();
            }
        }

    }
}