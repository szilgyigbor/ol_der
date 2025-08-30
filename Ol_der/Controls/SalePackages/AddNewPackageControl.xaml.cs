using Microsoft.EntityFrameworkCore;
using Ol_der.Controls.CustomerSearch;
using Ol_der.Controls.DetailedSearch;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Products;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Ol_der.Controls.SalePackages
{
    /// <summary>
    /// Interaction logic for AddNewSaleControl.xaml
    /// </summary>
    public partial class AddNewPackageControl : UserControl
    {
        private Window? _addNewProductWindow;
        private ShowAllPackageControl _showAllPackageControl;
        private PackageViewModel _packageViewModel;
        private int _saleId;
        private Sale _saleToSave;
        public Action OnFinished;

        public AddNewPackageControl()
        {
            InitializeComponent();
            _packageViewModel = new PackageViewModel();
            _showAllPackageControl = new ShowAllPackageControl();
            _saleId = -1;
            _saleToSave = new Sale();
            this.DataContext = _saleToSave;
            txtSaleDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        public async Task LoadExistsSale(int saleId)
        {
            _saleToSave = new Sale();
            _saleToSave.IsPackage = true;
            _saleToSave.PaymentType = PaymentType.Transfer;
            _saleId = saleId;

            if (_saleId > 0)
            {
                _saleToSave = await _packageViewModel.GetSaleAsync(_saleId);
                if (_saleToSave != null)
                {
                    txtExistsCustomerName.Text = _saleToSave.Customer?.Name;
                    txtCustomerName.Text = _saleToSave.CustomerName;
                    txtNotes.Text = _saleToSave.Notes;
                    txtTotalAmount.Text = _saleToSave.TotalAmount.ToString("0");
                    txtSaleDate.Text = _saleToSave.Date.ToString("yyyy-MM-dd HH:mm");
                    lstSaleItems.Items.Clear();
                    foreach (var item in _saleToSave.SaleItems)
                    {
                        lstSaleItems.Items.Add(item);
                    }
                }
            }
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
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztott elem!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private void CalculateTotal()
        {
            decimal total = 0;

            foreach (SaleItem item in lstSaleItems.Items)
            {
                total += item.Quantity * item.Price;
            }
            txtTotalAmount.Text = total.ToString();

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
                MessageBoxOkWindow messageBoxOkWindow = new("Írd be a cikkszámot, amit hozzá akarsz adni!");
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
                    MessageBoxWindow messageBoxWindow = new("Ez a termék már szerepel a listában, újból hozzáadjam?");
                    messageBoxWindow.ShowDialog();

                    if (messageBoxWindow.DialogResult == false)
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

            MessageBoxWindow messageBoxWindow = new($"Biztosan {saveOrModify} akarod a csomagot?");
            messageBoxWindow.ShowDialog();

            if (messageBoxWindow.DialogResult == false)
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

        private void DetailedSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ProductSelected -= OnProductSelected;
            searchWindow.ProductSelected += OnProductSelected;

            searchWindow.ShowDialog();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerSearchWindow customerSearchWindow = new CustomerSearchWindow();
            customerSearchWindow.CustomerSelected -= OnCustomerSelected;
            customerSearchWindow.CustomerSelected += OnCustomerSelected;
            customerSearchWindow.ShowDialog();
        }

        private void RemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            _saleToSave.CustomerId = null;
            _saleToSave.Customer = null;
            txtExistsCustomerName.Text = "";
        }

        private void OnCustomerSelected(object sender, Customer selectedCustomer)
        {
            txtExistsCustomerName.Text = selectedCustomer.Name;
            _saleToSave.CustomerId = selectedCustomer.CustomerId;
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
                MessageBoxWindow messageBoxWindow = new("Ez a termék már szerepel a listában, újból hozzáadjam?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == false)
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

        private async Task SaveSale()
        {
            decimal totalAmount;

            if (!decimal.TryParse(txtTotalAmount.Text, out totalAmount))
            {
                totalAmount = 0;
            }

            _saleToSave.CustomerName = txtCustomerName.Text;
            _saleToSave.TotalAmount = totalAmount;
            _saleToSave.Notes = txtNotes.Text;
            _saleToSave.IsPackage = true;
            _saleToSave.PaymentType = PaymentType.Transfer;

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
                await _packageViewModel.RemoveAllSaleItemsFromSaleAsync(_saleId);
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
                await _packageViewModel.UpdateSaleAsync(_saleToSave);
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
                await _packageViewModel.AddSaleAsync(_saleToSave);
            }

            _saleToSave = new Sale();
            _saleToSave.IsPackage = true;
            _saleToSave.PaymentType = PaymentType.Transfer;

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
            var upperItemNumber = itemNumber.ToUpper();
            return await _packageViewModel.SearchProductByItemNumberAsync(upperItemNumber);
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
