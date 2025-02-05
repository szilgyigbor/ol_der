using Ol_der.Controls.Suppliers;
using Ol_der.Data;
using System;
using System.Collections.Generic;
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
using Ol_der.Controls.DateFilter;
using Ol_der.Controls.Orders;
using System.IO;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for SalesControl.xaml
    /// </summary>
    public partial class SaleControl : UserControl
    {
        private object _filterForSales = 100;

        private AddNewSaleControl _addSaleControl;
        private ShowAllSaleControl _showAllSaleControl;
        private SaleRepository saleRepository;

        TextBlock loadingText = new TextBlock
        {
            Text = "BETÖLTÉS...",
            FontSize = 110,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        public SaleControl()
        {
            InitializeComponent();
            DataContext = this;
            _showAllSaleControl = new ShowAllSaleControl();
            _addSaleControl = new AddNewSaleControl();
            saleRepository = new SaleRepository();
            ShowFilteredSales();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            _addSaleControl = new AddNewSaleControl();
            ContentArea.Content = _addSaleControl;

            SalesTextBlock.Text = $"Új eladás";

            _addSaleControl.OnFinished -= async () =>
            {
                await ShowFilteredSales();
            };

            _addSaleControl.OnFinished += async () =>
            {
                await ShowFilteredSales();
            };
        }

        private async void ModifySale_Click(object sender, RoutedEventArgs e)
        {
            RefreshTitle();
            ContentArea.Content = _showAllSaleControl;
            int saleId = _showAllSaleControl.SaleIdToModify();

            if (saleId != -1)
            {
                _addSaleControl = new AddNewSaleControl();
                await _addSaleControl.LoadExistsSale(saleId);
                ContentArea.Content = _addSaleControl;

                SalesTextBlock.Text = $"Eladás módosítása";

                _addSaleControl.OnFinished -= async () =>
                {
                    await ShowFilteredSales();
                };

                _addSaleControl.OnFinished += async () =>
                {
                    await ShowFilteredSales();
                };
            }

            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Válassz ki egy eladást a módosításhoz!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private async Task ShowFilteredSales()
        {
            RefreshTitle();

            ContentArea.Content = loadingText;
            await _showAllSaleControl.RefreshSales(_filterForSales);
            ContentArea.Content = _showAllSaleControl;
        }

        private void RefreshTitle() 
        {
            if (_filterForSales is List<DateTime>)
            {
                var dates = _filterForSales as List<DateTime>;
                SalesTextBlock.Text = $"Eladások kezelése (megjelenített: {dates[0]:yyyy.MM.dd} - {dates[1]:yyyy.MM.dd})";
            }
            else
            {
                SalesTextBlock.Text = $"Eladások kezelése (megjelenített: {_filterForSales.ToString()})";
            }

        }

        private async void ShowFixedNumberOfSales_Click(object sender, RoutedEventArgs e)
        {
            InputSaleNumberWindow dialog = new InputSaleNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                _filterForSales = dialog.NumberResult ?? 200;
                await ShowFilteredSales();
            }
        }

        private async void DatePicker_Click(object sender, RoutedEventArgs e)
        {
            SetDateToFilter dateDialog = new SetDateToFilter();
            if (dateDialog.ShowDialog() == true)
            {
                _filterForSales = dateDialog.dateTimes;
                await ShowFilteredSales();
            }
        }

        private async void DatePickerForSummary_Click(object sender, RoutedEventArgs e)
        {
            SetDateToFilter dateDialog = new SetDateToFilter();
            if (dateDialog.ShowDialog() == true)
            {
                await GenerateSalesReport(dateDialog.dateTimes[0], dateDialog.dateTimes[1]);
            }
        }

        private async void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            await _showAllSaleControl.DeleteSale();
            await ShowFilteredSales();
        }

        private async void SearchSalesByProductNumber_Click(object sender, RoutedEventArgs e)
        {
            SearchDetailsWindow dialog = new SearchDetailsWindow();
            if (dialog.ShowDialog() == true)
            {
                string productNumber = dialog.ProductNumber;
                await ShowSalesByProductNumber(productNumber);
            }
        }

        private async Task ShowSalesByProductNumber(string productNumber)
        {
            ContentArea.Content = loadingText;
            SalesTextBlock.Text = $"Keresett termék cikkszáma: {productNumber}";
            await _showAllSaleControl.LoadSearchedSales(productNumber);
            ContentArea.Content = _showAllSaleControl;
        }

        public async Task GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            var sales = await saleRepository.GetSalesByDateRangeForRiportAsync(startDate, endDate);

            var groupedByMonth = sales
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(s => s.TotalAmount)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToList();

            var groupedByYear = sales
                .GroupBy(s => s.Date.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalRevenue = g.Sum(s => s.TotalAmount)
                })
                .OrderBy(g => g.Year)
                .ToList();

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Összegző riport.txt");

            var sb = new StringBuilder();

            sb.AppendLine($"Összegző riport ({startDate.ToShortDateString()} - {endDate.ToShortDateString()})");
            sb.AppendLine();

            sb.AppendLine("Éves bevétel:");
            foreach (var yearGroup in groupedByYear)
            {
                sb.AppendLine($"Év: {yearGroup.Year}, Bevétel: {yearGroup.TotalRevenue} Ft");
            }

            sb.AppendLine();
            sb.AppendLine("Havi bevétel:");
            foreach (var monthGroup in groupedByMonth)
            {
                sb.AppendLine($"Év: {monthGroup.Year}, Hónap: {monthGroup.Month}, Bevétel: {monthGroup.TotalRevenue} Ft");
            }

            File.WriteAllText(filePath, sb.ToString());

            MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"A riport sikeresen elmentve az asztalra: Összegző riport.txt");
            messageBoxOkWindow.ShowDialog();

        }


    }
}
