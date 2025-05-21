using Microsoft.EntityFrameworkCore.Internal;
using Ol_der.Controls.DetailedSearch;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Suppliers;
using Ol_der.Data;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Ol_der.Controls.Products
{
    /// <summary>
    /// Interaction logic for ProductsControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        private ProductViewModel _viewModel;
        private AddProductControl _addProductControl;
        private ShowAllProductControl _showAllProductControl;
        private ModifyProductControl _modifyProductControl;

        public Task InitializationTask { get; }

        public ProductControl()
        {
            InitializationTask = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            InitializeComponent();
            _viewModel = new ProductViewModel();
            this.DataContext = _viewModel;
            LoadProductCount();
            _addProductControl = new AddProductControl();
            _showAllProductControl = new ShowAllProductControl();
            _modifyProductControl = new ModifyProductControl();
            await ShowAllProduct();
        }


        public async void LoadProductCount()
        {
            _viewModel.ProductCount = await _viewModel.GetProductCountAsync();
        }

        public void Add_New_Product_Click(object sender, RoutedEventArgs e)
        {

            ContentArea.Content = _addProductControl;
            _addProductControl.OnProductAdded -= Add_Product;
            _addProductControl.OnProductAdded += Add_Product;
        }

        public async void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            _showAllProductControl.OnProductDeleted -= Delete_Product;
            _showAllProductControl.OnProductDeleted += Delete_Product;
            _showAllProductControl.DeleteProduct();
            await ShowAllProduct();
        }

        public async void Show_All_Product_Click(object sender, RoutedEventArgs e)
        {
            await ShowAllProduct();
        }

        public void Search_Product_Click(object sender, RoutedEventArgs e)
        {
            DetailedSearch(SearchProductById);
        }

        public async void Add_Product(Product newProduct)
        {
            bool isProductAdded = await _viewModel.AddProductAsync(newProduct);

            if (!isProductAdded)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("A termék cikkszáma már szerepel az adatbázisban!");
                messageBoxOkWindow.ShowDialog();

                return;
            }

            MessageBoxOkWindow messageBoxOkWindow1 = new("A termék sikeresen rögzítésre került!");
            messageBoxOkWindow1.ShowDialog();

            LoadProductCount();
            await ShowAllProduct();
        }

        private async void SearchProductById(object sender, Product product)
        {
            List<Product> products = new List<Product> { product };
            if (products == null || products.Count == 0)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs ilyen termék az adatbázisban!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            await _showAllProductControl.ShowAllProductAsync(products);
            ContentArea.Content = _showAllProductControl;
        }

        private async void Delete_Product(Product product)
        {
            await _viewModel.DeleteProductAsync(product);
            MessageBoxOkWindow messageBoxOkWindow = new("A termék sikeresen törölve lett!");
            messageBoxOkWindow.ShowDialog();
            LoadProductCount();
            await ShowAllProduct();
        }

        private async void Modify_Product_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = _showAllProductControl.ProductToModify();

            if (selectedProduct == null)
            {
                DetailedSearch(OnProductSelected);
            }

            else 
            {
                GetModifyWindow(selectedProduct);
            }
        }

        private async void GetModifyWindow(Product productToModify) 
        {
            _modifyProductControl.OnProductModified -= ModifyProduct;
            _modifyProductControl.OnProductModified += ModifyProduct;

            _modifyProductControl.SetDatasToModify(productToModify, await _viewModel.GetAllSupplierAsync());
            ContentArea.Content = _modifyProductControl;
        }


        private void DetailedSearch(EventHandler<Product> handler)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ProductSelected -= handler;
            searchWindow.ProductSelected += handler;

            searchWindow.ShowDialog();
        }

        private void OnProductSelected(object sender, Product productToModify)
        {
            GetModifyWindow(productToModify);
        }

        private async void ModifyProduct(Product modifiedProduct)
        {
            await _viewModel.UpdateProductAsync(modifiedProduct);
            MessageBoxOkWindow messageBoxOkWindow = new("A termék adatai sikeresen módosításra kerültek!");
            messageBoxOkWindow.ShowDialog();
            await ShowAllProduct();
        }

        public async Task ShowAllProduct()
        {
            _showAllProductControl = new();
            ContentArea.Content = _showAllProductControl;
            var products = await _viewModel.GetAllProductAsync();
            await _showAllProductControl.ShowAllProductAsync(products);
        }
        
    }
}
