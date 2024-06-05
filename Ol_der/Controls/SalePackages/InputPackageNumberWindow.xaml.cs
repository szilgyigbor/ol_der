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
using Ol_der.Controls.Orders;
using Ol_der.Models;

namespace Ol_der.Controls.SalePackages
{
    /// <summary>
    /// Interaction logic for InputPackageNumberWindow.xaml
    /// </summary>
    public partial class InputPackageNumberWindow : Window
    {
        public int? NumberResult { get; private set; }

        public InputPackageNumberWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumberInput.Text, out int number))
            {
                NumberResult = number;
                DialogResult = true;
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Kérlek, érvényes számot adj meg!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
