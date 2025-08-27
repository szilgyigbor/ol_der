using Ol_der.Controls.Suppliers;
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

namespace Ol_der.Controls.Customers
{
    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        private AddOrModifyCustomerControl _addCustomerControl = new();
        private ShowAllCustomerControl _showAllCustomerControl = new();

        public CustomerControl()
        {
            InitializeComponent();
        }

        private void Show_All_Customer()
        {
            ContentArea.Content = _showAllCustomerControl;
        }

        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _addCustomerControl;
        }

        private void Show_All_Customer_Click(object sender, RoutedEventArgs e)
        {
            Show_All_Customer();
        }
    }
}
