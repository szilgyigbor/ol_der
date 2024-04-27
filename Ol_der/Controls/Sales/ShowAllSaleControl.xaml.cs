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
            ShowAllSale();
        }

        public void ShowAllSale()
        {
            _viewModel = new SaleViewModel();
            Sales = _viewModel.GetAllSale();
            SalesListView.ItemsSource = Sales;
            _viewModel.Dispose();
        }

        public Sale SaleToModify()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                return SelectedSale;
            }

            return null;
        }

        public void DeleteSale()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                if (MessageBox.Show("Biztosan törölni szeretnéd ezt az eladást?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _viewModel = new SaleViewModel();
                    _viewModel.DeleteSale(SelectedSale);
                    _viewModel.Dispose();
                    ShowAllSale();
                }
            }
            else
            {
                MessageBox.Show("Válassz ki egy terméket a törléshez!");
            }

        }

    }
}
