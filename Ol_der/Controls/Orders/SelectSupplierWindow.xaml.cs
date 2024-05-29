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

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for SelectSupplierWindow.xaml
    /// </summary>
    public partial class SelectSupplierWindow : Window
    {
        private OrderViewModel _viewModel;
        private List<Supplier> _suppliers;
        public int supplierId;

        public SelectSupplierWindow()
        {
            InitializeComponent();
            _viewModel = new OrderViewModel();
            GetAllSupplier();
        }

        private async Task GetAllSupplier()
        {
            _suppliers = await _viewModel.GetAllSupplierAsync();
            LoadSuppliers(_suppliers);
        }


        private void LoadSuppliers(List<Supplier> suppliers)
        {
            suppliersComboBox.ItemsSource = suppliers;
            suppliersComboBox.SelectedIndex = 0;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            supplierId = (suppliersComboBox.SelectedItem as Supplier).SupplierId;
            DialogResult = true;
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
