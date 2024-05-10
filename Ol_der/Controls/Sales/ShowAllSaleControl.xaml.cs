using Ol_der.Models;
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
                if (MessageBox.Show("Biztosan törölni szeretnéd ezt az eladást?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await _viewModel.DeleteSaleAsync(SelectedSale);
                }
            }
            else
            {
                MessageBox.Show("Válassz ki egy terméket a törléshez!");
            }

        }

    }
}
