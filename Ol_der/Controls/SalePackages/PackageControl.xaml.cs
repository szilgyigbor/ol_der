using Ol_der.Controls.DateFilter;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Sales;
using Ol_der.Controls.Suppliers;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ol_der.Controls.SalePackages
{
    /// <summary>
    /// Interaction logic for SalesControl.xaml
    /// </summary>
    public partial class PackageControl : UserControl
    {
        private object _filterForPackages = 100;

        private AddNewPackageControl _addPackageControl;
        private ShowAllPackageControl _showAllPackageControl;

        TextBlock loadingText = new TextBlock
        {
            Text = "BETÖLTÉS...",
            FontSize = 110,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        public PackageControl()
        {
            InitializeComponent();
            _showAllPackageControl = new ShowAllPackageControl();
            _addPackageControl = new AddNewPackageControl();
            ShowFilteredPackages();
        }

        private void AddPackage_Click(object sender, RoutedEventArgs e)
        {
            _addPackageControl = new AddNewPackageControl();
            ContentArea.Content = _addPackageControl;
            PackagesTextBlock.Text = $"Új csomag";

            _addPackageControl.OnFinished -= async () =>
            {
                await ShowFilteredPackages();
            };

            _addPackageControl.OnFinished += async () =>
            {
                await ShowFilteredPackages();
            };
        }

        private async void ModifyPackage_Click(object sender, RoutedEventArgs e)
        {
            RefreshTitle();
            ContentArea.Content = _showAllPackageControl;
            int saleId = _showAllPackageControl.SaleIdToModify();
            if (saleId != -1)
            {
                _addPackageControl = new AddNewPackageControl();
                await _addPackageControl.LoadExistsSale(saleId);
                ContentArea.Content = _addPackageControl;
                PackagesTextBlock.Text = $"Csomag módosítása";

                _addPackageControl.OnFinished -= async () =>
                {
                    await ShowFilteredPackages();
                };

                _addPackageControl.OnFinished += async () =>
                {
                    await ShowFilteredPackages();
                };
            }

            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy csomagot a módosításhoz!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private async Task ShowFilteredPackages()
        {
            RefreshTitle();
            ContentArea.Content = loadingText;
            await _showAllPackageControl.RefreshSales(_filterForPackages);
            ContentArea.Content = _showAllPackageControl;
        }

        private void RefreshTitle() 
        {
            if (_filterForPackages is List<DateTime>)
            {
                var dates = _filterForPackages as List<DateTime>;
                PackagesTextBlock.Text = $"Csomagok kezelése (megjelenített: {dates[0]:yyyy.MM.dd} - {dates[1]:yyyy.MM.dd})";
            }
            else
            {
                PackagesTextBlock.Text = $"Csomagok kezelése (megjelenített: {_filterForPackages.ToString()})";
            }
        }

        private async void ShowFixedNumberOfPackages_Click(object sender, RoutedEventArgs e)
        {
            InputPackageNumberWindow dialog = new InputPackageNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                _filterForPackages = dialog.NumberResult ?? 200;
                await ShowFilteredPackages();
            }
        }

        private async void DatePicker_Click(object sender, RoutedEventArgs e)
        {
            SetDateToFilter dateDialog = new SetDateToFilter();
            if (dateDialog.ShowDialog() == true)
            {
                _filterForPackages = dateDialog.dateTimes;
                await ShowFilteredPackages();
            }
        }

        private async void btnDeletePackage_Click(object sender, RoutedEventArgs e)
        {
            await _showAllPackageControl.DeleteSale();
            await ShowFilteredPackages();
        }

        private async void SearchPackagesByProductNumber_Click(object sender, RoutedEventArgs e)
        {
            Sales.SearchDetailsWindow dialog = new Sales.SearchDetailsWindow();
            if (dialog.ShowDialog() == true)
            {
                var searchCriteria = dialog.SearchCriteria;
                await ShowSalesByCriteria(searchCriteria);
            }
        }

        private async Task ShowSalesByCriteria(Dictionary<string, string> SearchCriteria)
        {
            ContentArea.Content = loadingText;

            if (SearchCriteria == null)
            {
                PackagesTextBlock.Text = "Nincsenek megadott keresési feltételek.";
                return;
            }

            var labels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "CustomerName", "Név" },
                { "ProductNumber", "Cikkszám" },
                { "CustomerId", "Ügyfélazonosító" }
            };

            var parts = new List<string>();

            foreach (var kvp in labels)
            {
                if (!SearchCriteria.TryGetValue(kvp.Key, out var value))
                    continue;

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                var clean = value.Trim();

                parts.Add($"{kvp.Value}: {clean}");
            }

            string searchCriteriaText;

            if (parts.Count == 0)
            {
                searchCriteriaText = "Nincsenek megadott keresési feltételek.";
            }
            else
            {
                searchCriteriaText = "Keresési feltételek: " + string.Join(", ", parts);
            }

            PackagesTextBlock.Text = searchCriteriaText;

            await _showAllPackageControl.LoadSearchedSales(SearchCriteria);

            ContentArea.Content = _showAllPackageControl;
        }

    }
}
