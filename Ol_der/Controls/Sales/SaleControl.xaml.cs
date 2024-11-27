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

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for SalesControl.xaml
    /// </summary>
    public partial class SaleControl : UserControl
    {
        private object _filterForSales = 200;

        private AddNewSaleControl _addSaleControl;
        private ShowAllSaleControl _showAllSaleControl;

        public SaleControl()
        {
            InitializeComponent();
            DataContext = this;
            _showAllSaleControl = new ShowAllSaleControl();
            _addSaleControl = new AddNewSaleControl();
            ShowFilteredSales();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            _addSaleControl = new AddNewSaleControl();
            ContentArea.Content = _addSaleControl;

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
            ContentArea.Content = _showAllSaleControl;
            int saleId = _showAllSaleControl.SaleIdToModify();

            if (saleId != -1)
            {
                _addSaleControl = new AddNewSaleControl();
                await _addSaleControl.LoadExistsSale(saleId);
                ContentArea.Content = _addSaleControl;

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
            ContentArea.Content = "";
            SalesTextBlock.Text = $"BETÖLTÉS!";
            await _showAllSaleControl.RefreshSales(_filterForSales);
            ContentArea.Content = _showAllSaleControl;
            SalesTextBlock.Text = $"Eladások kezelése (megjelenített: {_filterForSales.ToString()})";
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
            SetDateToFilter dialog = new SetDateToFilter();
            if (dialog.ShowDialog() == true)
            {
                
            }
        }

        private async void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            await _showAllSaleControl.DeleteSale();
            await ShowFilteredSales();
        }

        private async void SearchSalesByProductNumber_Click(object sender, RoutedEventArgs e)
        {
            InputProductNumberWindow dialog = new InputProductNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                string productNumber = dialog.ProductNumber;
                await ShowSalesByProductNumber(productNumber);
            }
        }

        private async Task ShowSalesByProductNumber(string productNumber)
        {
            await _showAllSaleControl.LoadSearchedSales(productNumber);
            ContentArea.Content = _showAllSaleControl;
        }

    }
}
