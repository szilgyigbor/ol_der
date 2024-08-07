using Ol_der.Controls.Orders;
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

        private AddNewPackageControl _addPackageControl;
        private ShowAllPackageControl _showAllPackageControl;
        public PackageControl()
        {
            InitializeComponent();
            _showAllPackageControl = new ShowAllPackageControl();
            _addPackageControl = new AddNewPackageControl();
            ShowAllSale();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            _addPackageControl = new AddNewPackageControl();
            ContentArea.Content = _addPackageControl;
        }

        private async void ModifySale_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllPackageControl;
            int saleId = _showAllPackageControl.SaleIdToModify();

            if (saleId != -1)
            {
                _addPackageControl = new AddNewPackageControl();
                await _addPackageControl.LoadExistsSale(saleId);
                ContentArea.Content = _addPackageControl;
            }

            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy csomagot a módosításhoz!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private async Task ShowAllSale(int limit = 1000)
        {
            await _showAllPackageControl.RefreshSales(limit);
            ContentArea.Content = _showAllPackageControl;
        }

        private async void ShowAllSale_Click(object sender, RoutedEventArgs e)
        {
            InputPackageNumberWindow dialog = new InputPackageNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                int numberToShow = dialog.NumberResult ?? 1000;
                await ShowAllSale(numberToShow);
            }
        }

        private async void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            await _showAllPackageControl.DeleteSale();
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
            await _showAllPackageControl.LoadSearchedSales(productNumber);
            ContentArea.Content = _showAllPackageControl;
        }

    }
}
