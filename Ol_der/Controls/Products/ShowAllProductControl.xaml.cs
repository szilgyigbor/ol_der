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
    /// Interaction logic for ShowAllProductControl.xaml
    /// </summary>
    public partial class ShowAllProductControl : UserControl
    {
        private ProductViewModel _viewModel;
        public ShowAllProductControl()
        {
            InitializeComponent();
            _viewModel = new ProductViewModel();
            Show_All_Product();
            this.Unloaded -= OnUnloaded;
            this.Unloaded += OnUnloaded;
        }

        public void Show_All_Product()
        {
            var products = _viewModel.GetAllProduct();
            ProductsListView.ItemsSource = products;
        }

        public void DeleteProduct()
        {
            if (ProductsListView.SelectedItem is Product selectedProduct)
            {
                if (MessageBox.Show("Biztosan törölni szeretnéd ezt a terméker?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Product product = selectedProduct;
                    _viewModel.DeleteProduct(product);

                    MessageBox.Show("A termék sikeresen törölve lett!");

                    Show_All_Product();
                }
            }
            else
            {
                MessageBox.Show("Válassz ki egy terméket a törléshez!");
            }

        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _viewModel?.Dispose();
        }
    }
}
