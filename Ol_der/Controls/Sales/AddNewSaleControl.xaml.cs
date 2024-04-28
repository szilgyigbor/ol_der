using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
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
        private ApplicationDbContext _context;
        private ShowAllSaleControl _showAllSaleControl;
        private int? _saleId = null;
        private Sale _saleToSave;

        public AddNewSaleControl(int? saleId = null)
        {
            InitializeComponent();
            _context = ApplicationDbContextFactory.Create();
            _showAllSaleControl = new ShowAllSaleControl();
            _saleId = saleId;
            _saleToSave = new Sale();
            LoadPaymentTypes();
            LoadSaleIfExists();
            this.Unloaded -= OnUnloaded;
            this.Unloaded += OnUnloaded;
        }

        private void LoadSaleIfExists()
        {
            if (_saleId.HasValue)
            {
                _saleToSave = _context.Sales
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product)
                    .Include(s => s.SaleItems)
                        .ThenInclude(si => si.Product.Supplier).FirstOrDefault(s => s.SaleId == _saleId.Value);
                if (_saleToSave != null)
                {
                    txtCustomerName.Text = _saleToSave.CustomerName;
                    txtNotes.Text = _saleToSave.Notes;
                    cmbPaymentType.SelectedItem = _saleToSave.PaymentType;
                    lstSaleItems.Items.Clear();
                    foreach (var item in _saleToSave.SaleItems)
                    {
                        lstSaleItems.Items.Add(item);
                    }
                    CalculateTotal();
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _context?.Dispose();
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
            string saveOrModify = _saleId.HasValue ? "módosítani" : "menteni";

            MessageBoxResult result = MessageBox.Show($"Biztosan {saveOrModify} akarod az eladást?", "Eladás mentése", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            CalculateTotal();
            SaveSale();
            
            ClearFields();

            _showAllSaleControl.ShowAllSale();
        }

        private void SaveSale()
        {
            if (!_saleId.HasValue) 
            {
                _saleToSave.Date = DateTime.Now;
            }

            _saleToSave.CustomerName = txtCustomerName.Text;
            _saleToSave.PaymentType = (PaymentType)cmbPaymentType.SelectedItem;
            _saleToSave.TotalAmount = decimal.Parse(txtTotalAmount.Text);
            _saleToSave.Notes = txtNotes.Text;

            foreach (SaleItem item in lstSaleItems.Items)
            {
                _saleToSave.SaleItems.Add(item);
            }

            if (_saleId.HasValue)
            {
                _context.SaveChanges();
                MessageBox.Show("Eladás sikeresen módosítva!");
            }

            else 
            {
                _context.Sales.Add(_saleToSave);
                MessageBox.Show("Eladás sikeresen hozzáadva!");
            }
            
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
            var upperItemNumber = itemNumber.ToUpper();
            return _context.Products
                .FirstOrDefault(p => p.ItemNumber.ToUpper().Contains(upperItemNumber));
        }
    }
}
