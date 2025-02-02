using Microsoft.EntityFrameworkCore;
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

namespace Ol_der.Controls.Products
{
    /// <summary>
    /// Interaction logic for ModifyProductControl.xaml
    /// </summary>
    public partial class ModifyProductControl : UserControl
    {
        private List<Supplier> _suppliers;
        private ProductRepository _productRepository;

        public event Action<Product> OnProductModified;
        public Product _productToModify;
        

        public ModifyProductControl()
        {
            InitializeComponent();
            _productRepository = new ProductRepository();
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


        public async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBoxOkWindow messageBoxOkWindow = new("A név mező kitöltése kötelező!");
                messageBoxOkWindow.ShowDialog();

                return;
            }

            int selectedSupplierId = (int)suppliersComboBox.SelectedValue;


            Supplier selectedSupplier = _suppliers.Find(s => s.SupplierId == selectedSupplierId);
            if (selectedSupplier == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("A kiválasztott beszállító nem található.");
                messageBoxOkWindow.ShowDialog();

                return;
            }

            _productToModify.Name = nameTextBox.Text;
            _productToModify.ItemNumber = itemNumberTextBox.Text;
            _productToModify.Supplier = selectedSupplier;

            if ((await _productRepository.SearchProductByItemNumberAsync(_productToModify.ItemNumber))
                .Any(p => p.ProductId != _productToModify.ProductId))
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Ez a cikkszám már szerepel az adatbázisban!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            OnProductModified?.Invoke(_productToModify);

            _productToModify = new Product();
        }
    }
}
