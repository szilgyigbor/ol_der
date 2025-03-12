using Microsoft.EntityFrameworkCore;
using Ol_der.Data;
using Ol_der.Models;
using Ol_der.Controls.Orders;
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
using System.Globalization;
using Ol_der.Controls.DetailedSearch;
using Ol_der.Controls.Products;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for AddNewSaleControl.xaml
    /// </summary>
    public partial class AddNewSaleControl : UserControl
    {
        private Window? _addNewProductWindow;
        private ShowAllSaleControl _showAllSaleControl;
        private SaleViewModel _saleViewModel;
        private int _saleId;
        private Sale _saleToSave;

        public Action OnFinished;

        public AddNewSaleControl()
        {
            InitializeComponent();
            _saleViewModel = new SaleViewModel();
            _showAllSaleControl = new ShowAllSaleControl();
            _saleId = -1;
            _saleToSave = new Sale();
            this.DataContext = _saleToSave;
            txtSaleDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            LoadPaymentTypes();
        }

        public async Task LoadExistsSale(int saleId)
        {
            _saleToSave = new Sale();
            _saleId = saleId;

            if (_saleId > 0)
            {
                _saleToSave = await _saleViewModel.GetSaleAsync(_saleId);
                if (_saleToSave != null)
                {
                    txtCustomerName.Text = _saleToSave.CustomerName;
                    txtNotes.Text = _saleToSave.Notes;
                    txtSaleDate.Text = _saleToSave.Date.ToString("yyyy-MM-dd HH:mm");
                    cmbPaymentType.SelectedItem = _saleToSave.PaymentType;
                    txtTotalAmount.Text = _saleToSave.TotalAmount.ToString("0");
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
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nincs kiválasztott elem!");
                messageBoxOkWindow.ShowDialog();


            }
        }

        private void DetailedSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ProductSelected -= OnProductSelected;
            searchWindow.ProductSelected += OnProductSelected;

            searchWindow.ShowDialog();
        }

        private void OnProductSelected(object sender, Product selectedProduct)
        {
            bool productAlreadyAdded = false;
            foreach (SaleItem existingItem in lstSaleItems.Items)
            {
                if (existingItem.Product.ItemNumber == selectedProduct.ItemNumber)
                {
                    productAlreadyAdded = true;
                    break;
                }
            }

            if (productAlreadyAdded)
            {
                MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Ez a termék már szerepel a listában, újból hozzáadjam?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult != true)
                {
                    txtItemNumber.Text = "";
                    return;
                }
            }

            var saleItem = new SaleItem
            {
                ProductId = selectedProduct.ProductId,
                Product = selectedProduct,
                Quantity = 1,
                Price = 0
            };

            lstSaleItems.Items.Add(saleItem);
            txtItemNumber.Text = "";
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

        private async void btnSearchProduct_Click(object sender, RoutedEventArgs e)
        {
            var itemNumber = txtItemNumber.Text;
            if (string.IsNullOrWhiteSpace(itemNumber))
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Írd be a cikkszámot, amit hozzá akarsz adni!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            var product = await FindProductByItemNumberAsync(itemNumber);
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
                    MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Ez a termék már szerepel a listában, újból hozzáadjam?");
                    messageBoxWindow.ShowDialog();

                    if (messageBoxWindow.DialogResult != true)
                    {
                        txtItemNumber.Text = "";
                        return;
                    }
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
                MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Nincs ilyen termék! Rögzítsük?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    AddProductControl AddNewProductControl = new AddProductControl(itemNumber);

                    AddNewProductControl.OnProductAdded -= AddNewProductToSale;
                    AddNewProductControl.OnProductAdded += AddNewProductToSale;

                    _addNewProductWindow = new Window
                    {
                        Title = "UserControl Ablakban",
                        Content = AddNewProductControl,
                        Width = 900,
                        Height = 300
                    };

                    _addNewProductWindow.Show();
                }

            }
        }

        private async void btnSaveSale_Click(object sender, RoutedEventArgs e)
        {
            string saveOrModify = _saleId < 0 ? "menteni" : "módosítani";

            MessageBoxWindow messageBoxWindow = new MessageBoxWindow($"Biztosan {saveOrModify} akarod az eladást?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult != true)
            {
                return;
            }

            await SaveSale();

            OnFinished?.Invoke();
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OnFinished?.Invoke();
        }

        private async Task SaveSale()
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

            if (DateTime.TryParseExact(txtSaleDate.Text, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
            {
                _saleToSave.Date = newDate;
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("A dátum formátuma nem megfelelő. Kérlek, használd a következő formátumot: yyyy-MM-dd HH:mm");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            if (_saleId > 0)
            {
                _saleToSave.SaleItems.Clear();
                await _saleViewModel.RemoveAllSaleItemsFromSaleAsync(_saleId);
                foreach (SaleItem item in lstSaleItems.Items)
                {
                    _saleToSave.SaleItems.Add(new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IsOrdered = item.IsOrdered,
                        NeedToOrder = item.NeedToOrder
                    });
                }
                await _saleViewModel.UpdateSaleAsync(_saleToSave);

            }
            else
            {
                foreach (SaleItem item in lstSaleItems.Items)
                {
                    _saleToSave.SaleItems.Add(new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        IsOrdered = item.IsOrdered,
                        NeedToOrder = item.NeedToOrder
                    });
                }
                await _saleViewModel.AddSaleAsync(_saleToSave);

            }

            _saleToSave = new Sale();
            _saleId = -1;

            ClearFields();
        }

        private void ClearFields()
        {
            txtCustomerName.Text = "";
            txtItemNumber.Text = "";
            txtTotalAmount.Text = "";
            txtNotes.Text = "";
            lstSaleItems.Items.Clear();
        }

        private async Task<Product> FindProductByItemNumberAsync(string itemNumber)
        {
            return await _saleViewModel.SearchProductByItemNumberAsync(itemNumber);
        }

        public async void AddNewProductToSale(Product newProduct)
        {
            ProductViewModel viewModel = new ProductViewModel();
            bool isProductAdded = await viewModel.AddProductAsync(newProduct);

            if (!isProductAdded)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("A termék cikkszáma már szerepel az adatbázisban!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            _addNewProductWindow?.Close();

            var product = await FindProductByItemNumberAsync(newProduct.ItemNumber);
            if (product != null)
            {
                var saleItem = new SaleItem
                {
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = 1,
                    Price = 0
                };

                lstSaleItems.Items.Add(saleItem);
                txtItemNumber.Text = "";

                MessageBoxOkWindow messageBoxOkWindow1 = new("Sikeresen rögzítve és hozzáadva az eladáshoz!");
                messageBoxOkWindow1.ShowDialog();

            }

            else 
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Valami hina történt!");
                messageBoxOkWindow.ShowDialog();
            }
            
        }
    }
}
