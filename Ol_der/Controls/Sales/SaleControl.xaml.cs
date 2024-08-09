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

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for SalesControl.xaml
    /// </summary>
    public partial class SaleControl : UserControl
    {

        private AddNewSaleControl _addSaleControl;
        private ShowAllSaleControl _showAllSaleControl;
        public SaleControl()
        {
            InitializeComponent();
            _showAllSaleControl = new ShowAllSaleControl();
            _addSaleControl = new AddNewSaleControl();
            ShowAllSale();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            _addSaleControl = new AddNewSaleControl();
            ContentArea.Content = _addSaleControl;

            _addSaleControl.OnFinished -= async () =>
            {
                await ShowAllSale();
            };

            _addSaleControl.OnFinished += async () =>
            {
                await ShowAllSale();
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
                    await ShowAllSale();
                };

                _addSaleControl.OnFinished += async () =>
                {
                    await ShowAllSale();
                };
            }

            else
            {
                MessageBox.Show("Válassz ki egy eladást a módosításhoz!");
            }
        }

        private async Task ShowAllSale(int limit = 1000)
        {
            await _showAllSaleControl.RefreshSales(limit);
            ContentArea.Content = _showAllSaleControl;
        }

        private async void ShowAllSale_Click(object sender, RoutedEventArgs e)
        {
            InputSaleNumberWindow dialog = new InputSaleNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                int numberToShow = dialog.NumberResult ?? 1000;
                await ShowAllSale(numberToShow);
            }
        }

        private async void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            await _showAllSaleControl.DeleteSale();
            await ShowAllSale();
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
