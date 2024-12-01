using Ol_der.Controls.Sales;
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

namespace Ol_der.Controls.SalePackages
{
    internal class ShowAllPackageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PackageRepository _packageRepository;
        private ObservableCollection<Sale> _packages;

        public ObservableCollection<Sale> Packages
        {
            get { return _packages; }

            set
            {
                if (_packages != value)
                {
                    _packages = value;
                }
                OnPropertyChanged();
            }

        }
        public CollectionViewSource GroupedSales { get; private set; }

        public ShowAllPackageViewModel()
        {
            _packageRepository = new PackageRepository();
            Packages = new ObservableCollection<Sale>();
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
                    Packages = new ObservableCollection<Sale>(salesNumberList);
                    SetupGrouping();
                    break;

                case List<DateTime> dateTimes:
                    var salesDateList = await GetSalesByDateRangeAsync(dateTimes[0].Date, dateTimes[1].Date);
                    Packages = new ObservableCollection<Sale>(salesDateList);
                    SetupGrouping();
                    break;
            }
        }

        public async Task LoadSearchedSalesAsync(string itemNumber)
        {
            var salesList = await _packageRepository.GetSalesByItemNumberAsync(itemNumber);
            Packages = new ObservableCollection<Sale>(salesList);
            SetupGrouping();
        }


        private void SetupGrouping()
        {
            if (GroupedSales != null)
            {
                GroupedSales.Source = Packages;
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
            return await _packageRepository.GetAllSaleAsync(limit);
        }

        public async Task<List<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _packageRepository.GetSalesByDateRangeAsync(startDate, endDate);
        }

        public async Task DeleteSaleAsync(Sale sale)
        {
            await _packageRepository.DeleteSaleAsync(sale);
        }

    }
}
