using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ol_der.Controls.Sales
{
    internal class ShowAllSaleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly SaleRepository _saleRepository;
        private ObservableCollection<Sale> _sales;

        public ObservableCollection<Sale> Sales
        {
            get { return _sales; }
            
            set 
            {
                if (_sales != value)
                {
                    _sales = value;
                }
                OnPropertyChanged();
            }

        }
        public CollectionViewSource GroupedSales { get; private set; }

        public ShowAllSaleViewModel()
        {
            _saleRepository = new SaleRepository();
            Sales = new ObservableCollection<Sale>();
            GroupedSales = new CollectionViewSource();
        }

        public async Task RefreshData(object filterForSales)
        {
            await LoadDataAsync(filterForSales);
        }

        private async Task LoadDataAsync(object filterForSales)
        {
            switch (filterForSales)
            {
                case int number:
                    var salesNumberList = await GetAllSaleAsync(number);
                    Sales = new ObservableCollection<Sale>(salesNumberList);
                    SetupGrouping();
                    break;

                case List<DateTime> dateTimes:
                    var salesDateList = await GetSalesByDateRangeAsync(dateTimes[0].Date, dateTimes[1].Date);
                    Sales = new ObservableCollection<Sale>(salesDateList);
                    SetupGrouping();
                    break;
            }
        }

        public async Task LoadSearchedSalesAsync(string itemNumber)
        {
            var salesList = await _saleRepository.GetSalesByItemNumberAsync(itemNumber);
            Sales = new ObservableCollection<Sale>(salesList);
            SetupGrouping();
        }


        private void SetupGrouping()
        {
            if (GroupedSales != null)
            {
                GroupedSales.Source = Sales;
                GroupedSales.GroupDescriptions.Clear();
                GroupedSales.GroupDescriptions.Add(new PropertyGroupDescription("Date", new DateConverter()));
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Sale>> GetAllSaleAsync(int limit)
        {
            return await _saleRepository.GetAllSaleAsync(limit);
        }

        public async Task<List<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _saleRepository.GetSalesByDateRangeAsync(startDate, endDate);
        }

        public async Task DeleteSaleAsync(Sale sale)
        {
            await _saleRepository.DeleteSaleAsync(sale);
        }

    }
}
