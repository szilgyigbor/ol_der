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
        public WarrantyControl()
        {
            InitializeComponent();
            ShowAllWarranty();
        }

        public void ShowAllWarranty(int warrantyNumber = 100)
        {
            _showAllWarrantyControl = new ShowAllWarrantyControl();
            ContentArea.Content = _showAllWarrantyControl;
        }

        private void ShowAllWarranty_Click(object sender, RoutedEventArgs e)
        {
            ShowAllWarranty();
        }
    }
}
