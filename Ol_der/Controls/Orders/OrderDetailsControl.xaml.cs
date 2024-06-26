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

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for OrderDetailsControl.xaml
    /// </summary>
    public partial class OrderDetailsControl : UserControl
    {

        private OrderDetailsViewModel _viewModel;

        public OrderDetailsControl(int orderId)
        {
            InitializeComponent();
            _viewModel = new(orderId);
            this.DataContext = _viewModel;
        }
    }
}
