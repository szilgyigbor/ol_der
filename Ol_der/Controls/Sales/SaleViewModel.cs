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
    internal class SaleViewModel
    {
        private readonly SaleRepository _saleRepository;

        public SaleViewModel()
        {
            _saleRepository = new SaleRepository();
        }

        public void AddSale(Sale newSale)
        {
            _saleRepository.AddSale(newSale);
        }

        public List<Sale> GetAllSale()
        {
            return _saleRepository.GetAllSale();
        }

        public Product SearchProductByItemNumber(string itemNumber)
        {
            return _saleRepository.SearchProductByItemNumber(itemNumber);
        }

        public void DeleteSale(Sale sale)
        {
            _saleRepository.DeleteSale(sale);
        }

        public Sale GetSale(int saleId)
        {
            return _saleRepository.GetSale(saleId);
        }

        public void UpdateSale(Sale sale)
        {
            _saleRepository.UpdateSale(sale);
        }

        public void RemoveAllSaleItemsFromSale(int saleId)
        {
            _saleRepository.RemoveAllSaleItemsFromSale(saleId);
        }

    }
}
