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
    /// Interaction logic for SearchProductControl.xaml
    /// </summary>
    public partial class SearchProductControl : UserControl
    {
        public event Action<string> OnProductId;

        public SearchProductControl()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            OnProductId?.Invoke(itemNumberTextBox.Text);
        }
    }
}
