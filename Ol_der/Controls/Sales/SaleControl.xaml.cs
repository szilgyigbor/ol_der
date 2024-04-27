using Ol_der.Controls.Suppliers;
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
            _addSaleControl = new AddNewSaleControl();
            _showAllSaleControl = new ShowAllSaleControl();
            ShowAllSale();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _addSaleControl;
        }

        private void ShowAllSale()
        {
            ContentArea.Content = _showAllSaleControl;
        }

        private void ShowAllSale_Click(object sender, RoutedEventArgs e)
        {
            ShowAllSale();
        }
    }
}
