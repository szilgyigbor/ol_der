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

namespace Ol_der.Controls.DetailedSearch
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private SearchWindowViewModel _viewModel;

        public event EventHandler<Product> ProductSelected;

        public SearchWindow()
        {
            InitializeComponent();
            _viewModel = new SearchWindowViewModel();
            this.DataContext = _viewModel;
        }

        public void FinishSelectionClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel?.SelectedProduct != null)
            {
                ProductSelected?.Invoke(this, _viewModel.SelectedProduct);
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
            if (_viewModel.SelectedProduct != null)
            {
                FinishSelectionClick(sender, e);
            }
        }
    }
}
