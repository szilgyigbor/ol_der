using Ol_der.Controls.Orders;
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

namespace Ol_der.Controls.Warranties
{
    /// <summary>
    /// Interaction logic for WarrantyControl.xaml
    /// </summary>
    public partial class WarrantyControl : UserControl
    {
        private ShowAllWarrantyControl _showAllWarrantyControl;
        private AddOrUpdateWarrantyControl _addOrUpdateWarrantyControl;
        public WarrantyControl()
        {
            InitializeComponent();
            ShowAllWarranty();
        }

        public void ShowAllWarranty(int warrantyNumber = 100)
        {
            _showAllWarrantyControl = new ShowAllWarrantyControl(warrantyNumber);
            ContentArea.Content = _showAllWarrantyControl;
        }

        private void Refresh()
        {
            ShowAllWarranty();
        }

        private void ShowAllWarranty_Click(object sender, RoutedEventArgs e)
        {
            ShowAllWarranty();
        }

        private void AddNewWarranty_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxWindow MessageBox = new("Biztosan nyitunk új garanciás ügyet?");
            if (MessageBox.ShowDialog() == false)
            {
                return;
            }

            AddOrUpdateWarrantyControl _addOrUpdateWarrantyControl = new();
            _addOrUpdateWarrantyControl.OnWarrantyFinished -= Refresh;
            _addOrUpdateWarrantyControl.OnWarrantyFinished += Refresh;

            ContentArea.Content = _addOrUpdateWarrantyControl;
        }

        private void UpdateWarranty_Click(object sender, RoutedEventArgs e)
        {
            Warranty WarrantyToUpdate = _showAllWarrantyControl.GetSelectedWarranty();

            if (WarrantyToUpdate == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztva garanciális ügy!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            AddOrUpdateWarrantyControl _addOrUpdateWarrantyControl = new(WarrantyToUpdate);
            _addOrUpdateWarrantyControl.OnWarrantyFinished -= Refresh;
            _addOrUpdateWarrantyControl.OnWarrantyFinished += Refresh;
            ContentArea.Content = _addOrUpdateWarrantyControl;
        }
    }
}
