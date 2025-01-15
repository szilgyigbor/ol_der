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
            ContentArea.Content = _showAllPackageControl;
            int saleId = _showAllPackageControl.SaleIdToModify();

            if (saleId != -1)
            {
                _addPackageControl = new AddNewPackageControl();
                await _addPackageControl.LoadExistsSale(saleId);
                ContentArea.Content = _addPackageControl;

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

            if (_filterForPackages is List<DateTime>)
            {
                var dates = _filterForPackages as List<DateTime>;
                PackagesTextBlock.Text = $"Csomagok kezelése (megjelenített: {dates[0]:yyyy.MM.dd} - {dates[1]:yyyy.MM.dd})";
            }
            else
            {
                PackagesTextBlock.Text = $"Csomagok kezelése (megjelenített: {_filterForPackages.ToString()})";
            }

            ContentArea.Content = loadingText;
            await _showAllPackageControl.RefreshSales(_filterForPackages);
            ContentArea.Content = _showAllPackageControl;
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
            InputProductNumberWindow dialog = new InputProductNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                string productNumber = dialog.ProductNumber;
                await ShowPackagesByProductNumber(productNumber);
            }
        }

        private async Task ShowPackagesByProductNumber(string productNumber)
        {
            ContentArea.Content = loadingText;
            PackagesTextBlock.Text = $"Keresett termék cikkszáma: {productNumber}";
            await _showAllPackageControl.LoadSearchedSales(productNumber);
            ContentArea.Content = _showAllPackageControl;
        }

    }
}
