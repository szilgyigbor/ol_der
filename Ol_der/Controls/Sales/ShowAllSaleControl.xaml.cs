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
            ShowAllSale();
        }

        public void ShowAllSale()
        {
            Sales = _viewModel.GetAllSale();
            SalesListView.ItemsSource = Sales;
        }

        public int SaleIdToModify()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                return SelectedSale.SaleId;
            }

            return -1;
        }

        public void DeleteSale()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                if (MessageBox.Show("Biztosan törölni szeretnéd ezt az eladást?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteSale(SelectedSale);
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
