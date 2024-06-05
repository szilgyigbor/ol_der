using Ol_der.Models;
using Ol_der.Controls.Orders;
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
    /// Interaction logic for ShowAllSupplierControl.xaml
    /// </summary>
    public partial class ShowAllSupplierControl : UserControl
    {
        public ShowAllSupplierControl()
        {
            InitializeComponent();
        }

        public void DisplaySuppliers(List<Supplier> suppliers)
        {
            SuppliersListView.ItemsSource = suppliers;
        }
 
        public int SupplierIdToDelete()
        {
            if (SuppliersListView.SelectedItem is Supplier selectedSupplier)
            {
                MessageBoxWindow messageBoxWindow = new("Biztosan törölni szeretnéd ezt a beszállítót?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    Supplier supplier = selectedSupplier;
                    return supplier.SupplierId;
                }
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy beszállítót a törléshez!");
                messageBoxOkWindow.ShowDialog();
            }

            return -1;
        }

        public Supplier SupplierToModify()
        { 
            if (SuppliersListView.SelectedItem is Supplier selectedSupplier)
            {
                return selectedSupplier;
            }

            return null;
        }
    }
}
