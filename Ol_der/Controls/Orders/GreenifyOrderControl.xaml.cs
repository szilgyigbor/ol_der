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

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for GreenifyOrderControl.xaml
    /// </summary>
    public partial class GreenifyOrderControl : UserControl
    {
        private GreenifyOrderViewModel _viewModel;
        public GreenifyOrderControl(Order order)
        {
            InitializeComponent();
            _viewModel = new GreenifyOrderViewModel(order);
            this.DataContext = _viewModel;
        }
    }
}
