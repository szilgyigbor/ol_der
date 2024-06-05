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
    /// Interaction logic for ShowAllProductControl.xaml
    /// </summary>
    public partial class ShowAllProductControl : UserControl
    {
        public event Action<Product> OnProductDeleted;
        public ShowAllProductControl()
        {
            InitializeComponent();
        }

        public void ShowAllProduct(List<Product> products)
        {
            ProductsListView.ItemsSource = products;
        }

        public Product ProductToModify()
        {
            if (ProductsListView.SelectedItem is Product selectedProduct)
            {
                return selectedProduct;
            }

            return null;
        }

        public void DeleteProduct()
        {
            if (ProductsListView.SelectedItem is Product selectedProduct)
            {
                MessageBoxWindow messageBoxWindow = new("Biztosan törölni szeretnéd ezt a terméket?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    OnProductDeleted?.Invoke(selectedProduct);
                }

            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy terméket a törléshez!");
                messageBoxOkWindow.ShowDialog();
            }

        }
    }
}
