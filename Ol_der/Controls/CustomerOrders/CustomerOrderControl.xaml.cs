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

namespace Ol_der.Controls.CustomerOrders
{
    /// <summary>
    /// Interaction logic for CustomerOrderControl.xaml
    /// </summary>
    public partial class CustomerOrderControl : UserControl
    {
        private CustomerOrderRepository _customerOrderRepository;
        private ShowAllCustomerOrderControl _showAllCustomerOrderControl;

        public CustomerOrderControl()
        {
            InitializeComponent();
            ShowAllCustomerOrder();
            _customerOrderRepository = new();
        }

        public void ShowAllCustomerOrder(int customerOrderNumber = 100)
        {
            _showAllCustomerOrderControl = new ShowAllCustomerOrderControl(customerOrderNumber);
            ContentArea.Content = _showAllCustomerOrderControl;
        }
    }
}
