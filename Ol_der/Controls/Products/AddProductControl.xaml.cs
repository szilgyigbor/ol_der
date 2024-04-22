﻿using Ol_der.Models;
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
    /// Interaction logic for AddProductControl.xaml
    /// </summary>
    public partial class AddProductControl : UserControl
    {
        public event Action<Product> OnProductAdded;
        public AddProductControl(List<Supplier> suppliers)
        {
            InitializeComponent();
            LoadSuppliers(suppliers);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(itemNumberTextBox.Text) || suppliersComboBox.SelectedValue == null)
            {
                MessageBox.Show("A termék neve, cikkszáma és a beszállító kiválasztása kötelező!");
                return;
            }

            var newProduct = new Product
            {
                ItemNumber = itemNumberTextBox.Text,
                Name = nameTextBox.Text,
                SupplierId = (int)suppliersComboBox.SelectedValue
            };

            nameTextBox.Text = "";
            itemNumberTextBox.Text = "";

            OnProductAdded?.Invoke(newProduct);
           
        }

        private void LoadSuppliers(List<Supplier>suppliers)
        {
            suppliersComboBox.ItemsSource = suppliers;
            suppliersComboBox.SelectedIndex = 0;
        }

    }
}