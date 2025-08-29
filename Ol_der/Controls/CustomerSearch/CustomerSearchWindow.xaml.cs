using Ol_der.Controls.Orders;
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
using System.Windows.Shapes;

namespace Ol_der.Controls.CustomerSearch
{
    /// <summary>
    /// Interaction logic for CustomerSearchWindow.xaml
    /// </summary>
    public partial class CustomerSearchWindow : Window
    {
        private CustomerSearchWindowViewModel _viewModel;
        public event EventHandler<Customer> CustomerSelected;
        public CustomerSearchWindow()
        {
            InitializeComponent();
            _viewModel = new CustomerSearchWindowViewModel();
            this.DataContext = _viewModel;
        }

        public void FinishSelectionClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel?.SelectedCustomer != null)
            {
                CustomerSelected?.Invoke(this, _viewModel.SelectedCustomer);
                this.Close();
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Nincs kiválasztott termék!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedCustomer != null)
            {
                FinishSelectionClick(sender, e);
            }
        }
    }
}
