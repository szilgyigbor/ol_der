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
    /// Interaction logic for ShowAllOrderControl.xaml
    /// </summary>
    public partial class ShowAllOrderControl : UserControl
    {
        private OrderViewModel _viewModel;

        public ShowAllOrderControl(int limit = 100)
        {
            InitializeComponent();
            _viewModel = new OrderViewModel(limit);
            this.DataContext = _viewModel;
        }

        public int GetSelectedOrderId()
        {
            if (ProductsListView.SelectedItem is Order SelectedOrder)
            {
                return SelectedOrder.OrderId;
            }
            return -1;
        }

        public void SearchOrdersByProductNumber()
        {
            _viewModel.SearchOrdersByProductNumber();
        }

        public Task DeleteOrderByOrderIdAsync(int orderId)
        {
            return _viewModel.DeleteOrderByOrderIdAsync(orderId);
        }

    }
}
