using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Ol_der.Controls;
using Ol_der.Controls.Products;
using Ol_der.Controls.Sales;
using Ol_der.Controls.Suppliers;
using Ol_der.Controls.SalePackages;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Notes;
using Ol_der.Controls.Warranties;
using Ol_der.Controls.CustomerOrders;


namespace Ol_der
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            switch (button.Name)
            {
                case "btnSupplier":
                    SetActiveButtonColor("btnSupplier");
                    ContentArea.Content = new SupplierControl();
                    break;
                case "btnProduct":
                    SetActiveButtonColor("btnProduct");
                    ContentArea.Content = new ProductControl();
                    break;
                case "btnSale":
                    SetActiveButtonColor("btnSale");
                    ContentArea.Content = new SaleControl();
                    break;
                case "btnPackage":
                    SetActiveButtonColor("btnPackage");
                    ContentArea.Content = new PackageControl();
                    break;
                case "btnOrder":
                    SetActiveButtonColor("btnOrder");
                    ContentArea.Content = new OrderControl();
                    break;
                case "btnNote":
                    SetActiveButtonColor("btnNote");
                    ContentArea.Content = new NoteControl();
                    break;
                case "btnWarranty":
                    SetActiveButtonColor("btnWarranty");
                    ContentArea.Content = new WarrantyControl();
                    break;
                case "btnCustomerOrder":
                    SetActiveButtonColor("btnCustomerOrder");
                    ContentArea.Content = new CustomerOrderControl();
                    break;
            }
        }
        private void SetActiveButtonColor(string buttonName) 
        {
            var buttons = stackPanelButtons.Children.OfType<Button>().ToList();

            foreach (var button in buttons)
            {
                if (button.Name == buttonName) 
                {
                    button.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else 
                {
                    button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5F0F9"));
                }
            }
        }


        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan ki szeretnél lépni?");
            messageBoxWindow.ShowDialog();
            if (messageBoxWindow.DialogResult == true)
            {
                Application.Current.Shutdown();
            }
        }
    }
}