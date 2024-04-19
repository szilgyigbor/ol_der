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
    /// Interaction logic for AddSupplierControl.xaml
    /// </summary>
    public partial class AddSupplierControl : UserControl
    {
        public event Action<Supplier> OnSupplierAdded;

        public AddSupplierControl()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("A név mező kitöltése kötelező!");
                return;
            }

            Supplier newSupplier = new Supplier
            {
                Name = nameTextBox.Text,
                Address = addressTextBox.Text,
                Email = emailTextBox.Text,
                Phone = phoneTextBox.Text
            };

            OnSupplierAdded?.Invoke(newSupplier);

            var label = new Label
            {
                Content = "A beszállító sikeresen rögzítésre került!",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 22
            };
            this.Content = label;

        }
    }
}
