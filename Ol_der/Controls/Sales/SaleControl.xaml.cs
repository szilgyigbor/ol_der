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
            ContentArea.Content = _addSaleControl;
        }

        private void ModifySale_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllSaleControl;
            int saleId = _showAllSaleControl.SaleIdToModify();

            if (saleId != -1)
            {
                _addSaleControl = new AddNewSaleControl(saleId);
                ContentArea.Content = _addSaleControl;
            }

            else
            {
                MessageBox.Show("Válassz ki egy eladást a módosításhoz!");
            }
        }

        private void ShowAllSale()
        {
            _showAllSaleControl = new ShowAllSaleControl();
            ContentArea.Content = _showAllSaleControl;
        }

        private void ShowAllSale_Click(object sender, RoutedEventArgs e)
        {
            ShowAllSale();
        }

        private void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            _showAllSaleControl.DeleteSale();
            ShowAllSale();
        }

    }
}
