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
        private AddProductControl _addProductControl;
        private ShowAllProductControl _showAllProductControl;

        public ProductControl()
        {
            InitializeComponent();
            _addProductControl = null;
            _showAllProductControl = new ShowAllProductControl();
            Show_All_Product();
        }

        public void Add_New_Item_Click(object sender, RoutedEventArgs e)
        {
            if (_addProductControl == null)
            {
                _addProductControl = new AddProductControl();
                _addProductControl.Unloaded += (s, e) => _addProductControl = null;
            }

            ContentArea.Content = _addProductControl;
        }

        public void Show_All_Product_Click(object sender, RoutedEventArgs e)
        {
            Show_All_Product();
        }

        public void Show_All_Product()
        {
            if (_showAllProductControl == null)
            {
                _showAllProductControl = new ShowAllProductControl();
                _showAllProductControl.Unloaded += (s, e) => _showAllProductControl = null;
            }

            ContentArea.Content = _showAllProductControl;
        }

        public void Delete_Product_Click(object sender, RoutedEventArgs e)
        {
            if (_showAllProductControl == null)
            {
                _showAllProductControl = new ShowAllProductControl();
                _showAllProductControl.Unloaded += (s, e) => _showAllProductControl = null;
            }
            _showAllProductControl.DeleteProduct();

            Show_All_Product();
        }
    }
}
