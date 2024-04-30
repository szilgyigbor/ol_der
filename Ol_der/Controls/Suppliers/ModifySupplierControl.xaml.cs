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


namespace Ol_der.Controls.Suppliers
{
    /// <summary>
    /// Interaction logic for ModifySupplierControl.xaml
    /// </summary>
    public partial class ModifySupplierControl : UserControl
    {
        public event Action<Supplier> OnSupplierModified;

        private Supplier _supplierToModify;
        public ModifySupplierControl()
        {
            InitializeComponent();
            _supplierToModify = new Supplier();
        }

        public void GetDatasToModify(Supplier supplierToModify)
        {
            _supplierToModify = supplierToModify;

            nameTextBox.Text = supplierToModify.Name;
            addressTextBox.Text = supplierToModify.Address;
            emailTextBox.Text = supplierToModify.Email;
            phoneTextBox.Text = supplierToModify.Phone;
        }
    

        public void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("A név mező kitöltése kötelező!");
                return;
            }

            _supplierToModify.Name = nameTextBox.Text;
            _supplierToModify.Address = addressTextBox.Text;
            _supplierToModify.Email = emailTextBox.Text;
            _supplierToModify.Phone = phoneTextBox.Text;

            OnSupplierModified?.Invoke(_supplierToModify);

            _supplierToModify = new Supplier();

                  
        }
    }
}
