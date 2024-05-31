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
using Ol_der.Models;

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for AddNewOrderControl.xaml
    /// </summary>
    public partial class AddNewOrderControl : UserControl
    {
        private AddNewOrderViewModel _viewModel;

        public AddNewOrderControl(Order order, int supplierId)
        {
            InitializeComponent();
            _viewModel = new(order, supplierId);
            this.DataContext = _viewModel;
        }
    }
}
