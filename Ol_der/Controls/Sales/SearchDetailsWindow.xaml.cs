using Ol_der.Controls.Orders;
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
using System.Windows.Shapes;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for InputProductNumberWindow.xaml
    /// </summary>
    public partial class SearchDetailsWindow : Window
    {
        public Dictionary<string, string> SearchCriteria { get; private set; }

        public SearchDetailsWindow()
        {
            InitializeComponent();
            ProductNumberInput.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string productNumber = ProductNumberInput.Text.Trim();
            string customerName = CustomerNameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(productNumber) && string.IsNullOrWhiteSpace(customerName))
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Adj meg legalább egy keresési feltételt!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            SearchCriteria = new Dictionary<string, string>
            {
                { "ProductNumber", string.IsNullOrWhiteSpace(productNumber) ? null : productNumber },
                { "CustomerName", string.IsNullOrWhiteSpace(customerName) ? null : customerName }
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
