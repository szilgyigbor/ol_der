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

        /*public void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersListView.SelectedItem is Supplier selectedSupplier)
            {
                // Módosítás logikájának implementálása
                // Például megnyit egy új ablakot vagy UserControl-t a Supplier adatainak szerkesztéséhez
                var editWindow = new EditSupplierWindow(selectedSupplier);
                editWindow.Show();
            }
            else
            {
                MessageBox.Show("Válasszon ki egy beszállítót a módosításhoz!");
            }
        }*/

        public int DeleteSupplier()
        {
            if (SuppliersListView.SelectedItem is Supplier selectedSupplier)
            {
                if (MessageBox.Show("Biztosan törölni szeretné ezt a beszállítót?", "Megerősítés", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Supplier supplier = selectedSupplier;
                    return supplier.SupplierId;
                }
            }
            else
            {
                MessageBox.Show("Válasszon ki egy beszállítót a törléshez!");
            }

            return -1;
        }
    }
}
