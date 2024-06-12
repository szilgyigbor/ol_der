using Ol_der.Models;
using Ol_der.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Ol_der.Controls.Orders;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for ShowAllSaleControl.xaml
    /// </summary>
    public partial class ShowAllSaleControl : UserControl
    {
        private SaleViewModel _viewModel;
        public List<Sale> Sales { get; set; }
        public ShowAllSaleControl()
        {
            InitializeComponent();
            _viewModel = new SaleViewModel();
            this.DataContext = _viewModel;
        }


        public int SaleIdToModify()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                return SelectedSale.SaleId;
            }

            return -1;
        }

        public async Task RefreshSales(int limit)
        {
            await _viewModel.RefreshData(limit);
        }

        public async Task DeleteSale()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan törölni szeretnéd ezt az eladást?");
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    await _viewModel.DeleteSaleAsync(SelectedSale);
                }
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Válassz ki egy eladást a törléshez!");
            }

        }

        public async Task LoadSearchedSales(string productNumber)
        {
            await _viewModel.LoadSearchedSalesAsync(productNumber);
        }

    }
}
