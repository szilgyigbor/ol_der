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
            Unloaded -= Dispose;
            Unloaded += Dispose;
        }

        public void Dispose(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispose();
        }

        private void ShowAllSale()
        {
            Sales = _viewModel.GetAllSale();
            SalesListView.ItemsSource = Sales;
        }
    }
}
