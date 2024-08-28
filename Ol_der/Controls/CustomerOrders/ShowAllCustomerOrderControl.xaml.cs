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
    /// Interaction logic for ShowAllCustomerOrderControl.xaml
    /// </summary>
    public partial class ShowAllCustomerOrderControl : UserControl
    {
        private ShowAllCustomerOrderViewModel _viewModel;
        public ShowAllCustomerOrderControl(int limit)
        {
            InitializeComponent();
            _viewModel = new(limit);
            this.DataContext = _viewModel;
        }
    }
}
