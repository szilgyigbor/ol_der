using Microsoft.EntityFrameworkCore;
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

namespace Ol_der.Controls.Products
{
    /// <summary>
    /// Interaction logic for ModifyProductControl.xaml
    /// </summary>
    public partial class ModifyProductControl : UserControl
    {
        public event Action<Product> OnProductModified;
        public Product _productToModify;
        private List<Supplier> _suppliers;

        public ModifyProductControl()
        {
            InitializeComponent();
            _productToModify = new Product();
        }

        public void GetDatasToModify(Product productToModify, List<Supplier> suppliers)
        {
            _suppliers = suppliers;
            suppliersComboBox.ItemsSource = _suppliers;

            _productToModify = productToModify;

            nameTextBox.Text = productToModify.Name;
            itemNumberTextBox.Text = productToModify.ItemNumber;
            suppliersComboBox.SelectedIndex = _suppliers.FindIndex(s => s.SupplierId == productToModify.SupplierId);
        }


        public void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("A név mező kitöltése kötelező!");
                return;
            }

            int selectedSupplierId = (int)suppliersComboBox.SelectedValue;


            Supplier selectedSupplier = _suppliers.Find(s => s.SupplierId == selectedSupplierId);
            if (selectedSupplier == null)
            {
                MessageBox.Show("A kiválasztott beszállító nem található.");
                return;
            }

            _productToModify.Name = nameTextBox.Text;
            _productToModify.ItemNumber = itemNumberTextBox.Text;
            _productToModify.Supplier = selectedSupplier; 

            OnProductModified?.Invoke(_productToModify);
        }
    }
}
