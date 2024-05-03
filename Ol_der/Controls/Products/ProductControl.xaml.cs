using Microsoft.EntityFrameworkCore.Internal;
using Ol_der.Controls.Suppliers;
using Ol_der.Data;
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



        public ProductControl()
        {
            InitializeComponent();
            _viewModel = new ProductViewModel();
            this.DataContext = _viewModel;
            LoadProductCount();
            _addProductControl = new AddProductControl(GetAllSupplier());
            _showAllProductControl = new ShowAllProductControl();
            _modifyProductControl = new ModifyProductControl();
            _searchProductControl = new SearchProductControl();
            ShowAllProduct();
        }

        public void LoadProductCount()
        {
            _viewModel.ProductCount = _viewModel.GetProductCount();
        }

        public void Add_New_Product_Click(object sender, RoutedEventArgs e)
        {

            ContentArea.Content = _addProductControl;
            _addProductControl.OnProductAdded -= Add_Product;
            _addProductControl.OnProductAdded += Add_Product;
        }

        public void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            _showAllProductControl.DeleteProduct();
            ShowAllProduct();
            _showAllProductControl.OnProductDeleted -= Delete_Product;
            _showAllProductControl.OnProductDeleted += Delete_Product;
        }

        public void Show_All_Product_Click(object sender, RoutedEventArgs e)
        {
            ShowAllProduct();
        }

        public void Search_Product_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _searchProductControl;
            _searchProductControl.OnProductId -= SearchProductById;
            _searchProductControl.OnProductId += SearchProductById;
        }

        public void Add_Product(Product newProduct)
        {
            bool isProductAdded = _viewModel.AddProduct(newProduct);

            if (!isProductAdded)
            {
                MessageBox.Show("A termék cikkszáma már szerepel az adatbázisban!");
                return;
            }

            MessageBox.Show("A termék sikeresen rögzítésre került!");
            LoadProductCount();
            ShowAllProduct();
        }

        private void SearchProductById(string productId)
        {
            List<Product> product = _viewModel.SearchProductByItemNumber(productId);
            if (product == null)
            {
                MessageBox.Show("Nincs ilyen termék az adatbázisban!");
                return;
            }

            _showAllProductControl.ShowAllProduct(product);
            ContentArea.Content = _showAllProductControl;
        }

        private void Delete_Product(Product product)
        {
            _viewModel.DeleteProduct(product);
            MessageBox.Show("A termék sikeresen törölve lett!");
            LoadProductCount();
            ShowAllProduct();
        }

        private void Modify_Product_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = _showAllProductControl.ProductToModify();

            if (selectedProduct == null)
            {
                MessageBox.Show("Válassz ki egy terméket a módosításhoz!");
                ContentArea.Content = _showAllProductControl;
                return;
            }

            _modifyProductControl.OnProductModified -= ModifyProduct;
            _modifyProductControl.OnProductModified += ModifyProduct;

            _modifyProductControl.GetDatasToModify(selectedProduct, GetAllSupplier());
            ContentArea.Content = _modifyProductControl;
        }

        private void ModifyProduct(Product modifiedProduct)
        {
            _viewModel.UpdateProduct(modifiedProduct);
            MessageBox.Show("A termék adatai sikeresen módosításra kerültek!");
            ShowAllProduct();
        }

        public void ShowAllProduct()
        {
            _showAllProductControl = new ShowAllProductControl();
            ContentArea.Content = _showAllProductControl;
            _showAllProductControl.ShowAllProduct(GetAllProduct());
        }

        public List<Supplier> GetAllSupplier()
        {
            return _viewModel.GetAllSupplier();
        }

        public List<Product> GetAllProduct()
        {
            return _viewModel.GetAllProduct();
        }

        

        
    }
}
