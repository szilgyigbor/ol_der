using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for MessageBoxOkWindow.xaml
    /// </summary>
    public partial class MessageBoxOkWindow : Window
    {
        public MessageBoxOkWindow(string message)
        {
            InitializeComponent();
            SetMessage(message);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void SetMessage(string message)
        {
            messageToShow.Text = message;
        }
    }
}
