using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AddNewSaleControl.xaml
    /// </summary>
    public partial class AddNewSaleControl : UserControl
    {
        private SaleViewModel _viewModel;

        public AddNewSaleControl()
        {
            InitializeComponent();
            _viewModel = new SaleViewModel();

            LoadPaymentTypes();
            
            Unloaded -= Dispose;
            Unloaded += Dispose;
        }

        public void Dispose(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispose();
        }

        public void LoadPaymentTypes()
        {
            cmbPaymentType.ItemsSource = Enum.GetValues(typeof(PaymentType));
            cmbPaymentType.SelectedIndex = 0;
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            CalculateTotal();
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (lstSaleItems.SelectedItem is SaleItem selected)
            {
                lstSaleItems.Items.Remove(selected);
                CalculateTotal();
            }

            else if (lstSaleItems.SelectedItem == null)
            {
                MessageBox.Show("Nincs kiválasztott elem!");
            }
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            decimal RnTotal = 0;

            foreach (SaleItem item in lstSaleItems.Items)
            {
                total += item.Quantity * item.Price;
            }
            txtTotalAmount.Text = total.ToString();

            RnTotal = total - total * 0.10m;

            lblDiscountedTotal.Content = $"(Recept nélkül: {RnTotal:N0})";
        }

        private void txtItemNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearchProduct_Click(this, new RoutedEventArgs());
            }
        }

        private void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            var itemNumber = txtItemNumber.Text;

            if (string.IsNullOrWhiteSpace(itemNumber))
            {
                MessageBox.Show("Írd be a cikkszámot, amit hozzá akarsz adni!");
                return;
            }

            var product = FindProductByItemNumber(itemNumber);
            if (product != null)
            {
                var saleItem = new SaleItem { Product = product, Quantity = 1, Price = 0 };
                lstSaleItems.Items.Add(saleItem);

                txtItemNumber.Text = "";
            }
            else
            {
                MessageBox.Show("Nincs ilyen termék!");
            }
        }

        private void btnSaveSale_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Biztosan el akarod menteni az eladást?", "Eladás mentése", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            SaveSale();
            MessageBox.Show("Eladás sikeresen hozzáadva!");
            ClearFields();
        }

        private void SaveSale()
        {
            
            Sale sale = new Sale()
            {
                CustomerName = txtCustomerName.Text,
                Date = DateTime.Now,
                PaymentType = (PaymentType)cmbPaymentType.SelectedItem,
                TotalAmount = decimal.Parse(txtTotalAmount.Text),
                Notes = txtNotes.Text,
                SaleItems = new List<SaleItem>()
            };

            foreach (SaleItem item in lstSaleItems.Items)
            {
                sale.SaleItems.Add(item);
            }
   
            _viewModel.AddSale(sale);
        }

        private void ClearFields()
        {
            txtCustomerName.Text = "";
            txtItemNumber.Text = "";
            txtTotalAmount.Text = "";
            txtNotes.Text = "";
            lstSaleItems.Items.Clear();
        }

        private Product FindProductByItemNumber(string itemNumber)
        {
            Product product = _viewModel.SearchProductByItemNumber(itemNumber);
            return product;
        }
    }
}
