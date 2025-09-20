using Ol_der.Controls.CustomerSearch;
using Ol_der.Controls.Orders;
using Ol_der.Models;
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
        public string CustomerId { get; private set; }

        public SearchDetailsWindow()
        {
            InitializeComponent();
            ProductNumberInput.Focus();
        }

        private void SelectCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerSearchWindow customerSearchWindow = new CustomerSearchWindow();
            customerSearchWindow.CustomerSelected -= OnCustomerSelected;
            customerSearchWindow.CustomerSelected += OnCustomerSelected;
            customerSearchWindow.ShowDialog();
        }

        private void OnCustomerSelected(object sender, Customer selectedCustomer)
        {

            txtExistsCustomerName.Text = selectedCustomer.Name;
            CustomerId = selectedCustomer.CustomerId.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string productNumber = ProductNumberInput.Text.Trim();
            string customerName = CustomerNameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(productNumber) && string.IsNullOrWhiteSpace(customerName) && CustomerId == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Adj meg legalább egy keresési feltételt!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            SearchCriteria = new Dictionary<string, string>
            {
                { "ProductNumber", string.IsNullOrWhiteSpace(productNumber) ? null : productNumber },
                { "CustomerName", string.IsNullOrWhiteSpace(customerName) ? null : customerName },
                { "CustomerId", string.IsNullOrWhiteSpace(CustomerId) ? null : CustomerId }
            };

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
