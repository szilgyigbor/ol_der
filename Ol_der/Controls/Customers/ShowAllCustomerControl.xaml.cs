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
    /// Interaction logic for ShowAllCustomerControl.xaml
    /// </summary>
    public partial class ShowAllCustomerControl : UserControl
    {
        private ShowAllCustomerViewModel _viewModel;
        public ShowAllCustomerControl()
        {
            InitializeComponent();
            _viewModel = new ShowAllCustomerViewModel();
            this.DataContext = _viewModel;
        }

        public int GetSelectedCustomerId()
        {
            if (CustomersListView.SelectedItem is Models.Customer selectedCustomer)
            {
                return selectedCustomer.CustomerId;
            }
            return -1;
        }
    }
}
