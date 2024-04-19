using Ol_der.Data;
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

namespace Ol_der.Controls.Suppliers
{
    /// <summary>
    /// Interaction logic for SupplierControl.xaml
    /// </summary>
    public partial class SupplierControl : UserControl
    {
        private SupplierViewModel _viewModel;

        public SupplierControl()
        {
            InitializeComponent();
            _viewModel = new SupplierViewModel();
            this.Unloaded += OnUnloaded;
        }

        private void Add_Supplier_Click(object sender, RoutedEventArgs e)
        {
            var addControl = new AddSupplierControl();
            addControl.OnSupplierAdded += _viewModel.AddSupplier;
            ContentArea.Content = addControl;
        }

        private void Show_All_Supplier_Click(object sender, RoutedEventArgs e)
        {
            var showAllControl = new ShowAllSupplierControl();
            showAllControl.DisplaySuppliers(GetSuppliers());
            ContentArea.Content = showAllControl;
        }

        private List<Supplier> GetSuppliers()
        {
            return _viewModel.GetAllSupplier();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Dispose();
        }

    }
}
