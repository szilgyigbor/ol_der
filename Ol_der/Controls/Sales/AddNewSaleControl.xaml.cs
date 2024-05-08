﻿using Microsoft.EntityFrameworkCore;
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
        private ShowAllSaleControl _showAllSaleControl;
        private SaleViewModel _saleViewModel;
        private int _saleId;
        private Sale _saleToSave;

        public AddNewSaleControl()
        {
            InitializeComponent();
            _saleViewModel = new SaleViewModel();
            _showAllSaleControl = new ShowAllSaleControl();
            _saleId = -1;
            _saleToSave = new Sale();
            this.DataContext = _saleToSave;
            LoadPaymentTypes();
        }

        public void LoadExistsSale(int saleId)
        {
            _saleToSave = new Sale();
            _saleId = saleId;

            if (_saleId > 0)
            {
                _saleToSave = _saleViewModel.GetSale(_saleId);
                if (_saleToSave != null)
                {
                    txtCustomerName.Text = _saleToSave.CustomerName;
                    txtNotes.Text = _saleToSave.Notes;
                    cmbPaymentType.SelectedItem = _saleToSave.PaymentType;
                    txtTotalAmount.Text = _saleToSave.TotalAmount.ToString();
                    lstSaleItems.Items.Clear();
                    foreach (var item in _saleToSave.SaleItems)
                    {
                        lstSaleItems.Items.Add(item);
                    }
                    chkIsTransactionProcessed.IsChecked = _saleToSave.IsCardTransactionProcessed;
                }
            }
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

                bool productAlreadyAdded = false;
                foreach (SaleItem existingItem in lstSaleItems.Items)
                {
                    if (existingItem.Product.ItemNumber == product.ItemNumber)
                    {
                        productAlreadyAdded = true;
                        break;
                    }
                }

                if (productAlreadyAdded)
                {
                    MessageBox.Show("Ez a termék már szerepel a listában, növeld a darabszámot!");
                    txtItemNumber.Text = "";
                    return;
                }

                var saleItem = new SaleItem
                {
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = 1,
                    Price = 0
                };

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
            string saveOrModify = _saleId < 0 ? "menteni" : "módosítani";

            MessageBoxResult result = MessageBox.Show($"Biztosan {saveOrModify} akarod az eladást?", "Eladás mentése", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            SaveSale();
            ClearFields();
            _showAllSaleControl.ShowAllSale();
        }

        private void SaveSale()
        {
            decimal totalAmount;

            if (!decimal.TryParse(txtTotalAmount.Text, out totalAmount))
            {
                totalAmount = 0;
            }

            _saleToSave.CustomerName = txtCustomerName.Text;
            _saleToSave.PaymentType = (PaymentType)cmbPaymentType.SelectedItem;
            _saleToSave.TotalAmount = totalAmount;
            _saleToSave.Notes = txtNotes.Text;
            _saleToSave.IsCardTransactionProcessed = chkIsTransactionProcessed.IsChecked ?? false;

            if (_saleId > 0)
            {
                _saleToSave.SaleItems.Clear();
                _saleViewModel.RemoveAllSaleItemsFromSale(_saleId);
                foreach (SaleItem item in lstSaleItems.Items)
                {
                    _saleToSave.SaleItems.Add(new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IsOrdered = item.IsOrdered
                    });
                }
                _saleViewModel.UpdateSale(_saleToSave);
                MessageBox.Show("Eladás sikeresen módosítva!");
            }
            else
            {
                _saleToSave.Date = DateTime.Now;
                foreach (SaleItem item in lstSaleItems.Items)
                {
                    _saleToSave.SaleItems.Add(new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IsOrdered = item.IsOrdered
                    });
                }
                _saleViewModel.AddSale(_saleToSave);
                MessageBox.Show("Eladás sikeresen hozzáadva!");
            }

            _saleToSave = new Sale();
            _saleId = -1;
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
            return _saleViewModel.SearchProductByItemNumber(upperItemNumber);
        }
    }
}
