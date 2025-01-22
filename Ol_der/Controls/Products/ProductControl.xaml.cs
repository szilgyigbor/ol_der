﻿using Microsoft.EntityFrameworkCore.Internal;
using Ol_der.Controls.Suppliers;
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
        private SearchProductControl _searchProductControl;

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
            _addProductControl = new AddProductControl(await _viewModel.GetAllSupplierAsync());
            _showAllProductControl = new ShowAllProductControl();
            _modifyProductControl = new ModifyProductControl();
            _searchProductControl = new SearchProductControl();
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
            ContentArea.Content = _searchProductControl;
            _searchProductControl.OnProductId -= SearchProductById;
            _searchProductControl.OnProductId += SearchProductById;
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

        private async void SearchProductById(string productId)
        {
            List<Product> product = await _viewModel.SearchProductByItemNumberAsync(productId);
            if (product == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs ilyen termék az adatbázisban!");
                messageBoxOkWindow.ShowDialog();

                return;
            }

            _showAllProductControl.ShowAllProduct(product);
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
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy terméket a módosításhoz!");
                messageBoxOkWindow.ShowDialog();
                ContentArea.Content = _showAllProductControl;
                return;
            }

            _modifyProductControl.OnProductModified -= ModifyProduct;
            _modifyProductControl.OnProductModified += ModifyProduct;

            _modifyProductControl.GetDatasToModify(selectedProduct,await _viewModel.GetAllSupplierAsync());
            ContentArea.Content = _modifyProductControl;
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
            ContentArea.Content = _showAllProductControl;
            _showAllProductControl.ShowAllProduct(await _viewModel.GetAllProductAsync());
        }
        
    }
}
