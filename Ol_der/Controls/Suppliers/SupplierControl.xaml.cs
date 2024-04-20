﻿using Ol_der.Data;
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
        private AddSupplierControl _addSupplierControl;
        private ShowAllSupplierControl _showAllSupplierControl;
        private ModifySupplierControl _modifySupplierControl;

        public SupplierControl()
        {
            InitializeComponent();
            _viewModel = new SupplierViewModel();
            _addSupplierControl = new AddSupplierControl();
            _showAllSupplierControl = new ShowAllSupplierControl();
            _modifySupplierControl = new ModifySupplierControl();
            Show_All_Supplier();
            this.Unloaded += OnUnloaded;
        }

        private void Add_Supplier_Click(object sender, RoutedEventArgs e)
        {
            _addSupplierControl.OnSupplierAdded += _viewModel.AddSupplier;
            ContentArea.Content = _addSupplierControl;
        }

        private void Show_All_Supplier_Click(object sender, RoutedEventArgs e)
        {
            Show_All_Supplier();
        }

        private void Show_All_Supplier()
        {
            _showAllSupplierControl.DisplaySuppliers(GetSuppliers());
            ContentArea.Content = _showAllSupplierControl;
        }

        private void Delete_Supplier_Click(object sender, RoutedEventArgs e)
        {
            int supplierId = _showAllSupplierControl.SupplierIdToDelete();

            if (supplierId >= 0)
            {
                _viewModel.RemoveSupplier(supplierId);
                Show_All_Supplier();
            }
        }

        private void Modify_Supplier_Click(object sender, RoutedEventArgs e)
        {
            Supplier selectedSupplier = _showAllSupplierControl.SupplierToModify();

            if (selectedSupplier == null)
            {
                MessageBox.Show("Válasszon ki egy beszállítót a módosításhoz!");
                return;
            }

            _modifySupplierControl.OnSupplierModified += ModifySupplier;

            Show_All_Supplier();
        }

        private void ModifySupplier(Supplier modifiedSupplier)
        {
            _viewModel.ModifySupplier(modifiedSupplier);
            _showAllSupplierControl.DisplaySuppliers(GetSuppliers());
            MessageBox.Show("A beszállító adatai sikeresen módosításra kerültek!");
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
