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
using System.Windows.Shapes;

namespace Ol_der.Controls.SalePackages
{
    /// <summary>
    /// Interaction logic for InputProductNumberWindow.xaml
    /// </summary>
    public partial class InputProductNumberWindow : Window
    {
        public string ProductNumber { get; private set; }

        public InputProductNumberWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ProductNumber = ProductNumberInput.Text;
            if (string.IsNullOrWhiteSpace(ProductNumber))
            {
                MessageBox.Show("Add meg a cikkszámot!");
                return;
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
