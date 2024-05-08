using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Threading.Tasks;

namespace Ol_der.Controls.Sales
{
    internal class SaleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SaleRepository _saleRepository;
        public ObservableCollection<Sale> Sales { get; private set; }
        public CollectionViewSource GroupedSales { get; private set; }

        public SaleViewModel()
        {
            _saleRepository = new SaleRepository();
            Sales = new ObservableCollection<Sale>();
            GroupedSales = new CollectionViewSource();
            LoadDataAsync();
        }

        public async Task RefreshData() 
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var salesData = await GetAllSaleAsync();
            Sales.Clear();
            foreach (var sale in salesData)
            {
                Sales.Add(sale);
            }
            SetupGrouping();
        }

        private void SetupGrouping()
        {
            if (GroupedSales != null)
            {
                GroupedSales.Source = Sales;
                GroupedSales.GroupDescriptions.Clear();
                GroupedSales.GroupDescriptions.Add(new PropertyGroupDescription("Date", new DateToDayConverter()));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public async Task AddSaleAsync(Sale newSale)
        {
            await _saleRepository.AddSaleAsync(newSale);
        }

        public async Task<List<Sale>> GetAllSaleAsync()
        {
            return await _saleRepository.GetAllSaleAsync();
        }

        public async Task<Product> SearchProductByItemNumberAsync(string itemNumber)
        {
            return await _saleRepository.SearchProductByItemNumberAsync(itemNumber);
        }

        public async Task DeleteSaleAsync(Sale sale)
        {
            await _saleRepository.DeleteSaleAsync(sale);
        }

        public async Task<Sale> GetSaleAsync(int saleId)
        {
            return await _saleRepository.GetSaleAsync(saleId);
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            await _saleRepository.UpdateSaleAsync(sale);
        }

        public async Task RemoveAllSaleItemsFromSaleAsync(int saleId)
        {
            await _saleRepository.RemoveAllSaleItemsFromSaleAsync(saleId);
        }

    }
}
