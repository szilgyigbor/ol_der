using Ol_der.Controls.Orders;
using Ol_der.Controls.Suppliers;
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

namespace Ol_der.Controls.Customers
{
    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        private AddOrModifyCustomerControl _addCustomerControl = new();
        private ShowAllCustomerControl _showAllCustomerControl = new();
        private CustomerRepository _repository = new();

        public CustomerControl()
        {
            InitializeComponent();
            Show_All_Customer();
        }

        private void Show_All_Customer()
        {
            _showAllCustomerControl = new ShowAllCustomerControl();
            ContentArea.Content = _showAllCustomerControl;
        }

        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            _addCustomerControl = new AddOrModifyCustomerControl();
            ContentArea.Content = _addCustomerControl;
            _addCustomerControl.OnFinished -= Show_All_Customer;
            _addCustomerControl.OnFinished += Show_All_Customer;
        }

        private void Show_All_Customer_Click(object sender, RoutedEventArgs e)
        {
            Show_All_Customer();
        }

        private void Modify_Customer_Click(object sender, RoutedEventArgs e)
        {
            int selectedCustomerId = _showAllCustomerControl.GetSelectedCustomerId();
            if (selectedCustomerId != -1)
            {
                _addCustomerControl = new AddOrModifyCustomerControl(selectedCustomerId);
                ContentArea.Content = _addCustomerControl;
                _addCustomerControl.OnFinished -= Show_All_Customer;
                _addCustomerControl.OnFinished += Show_All_Customer;
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Válassz ki egy ügyfelet a módosításhoz!");
                messageBoxOkWindow.ShowDialog();

            }
        }

        private void Delete_Customer_Click(object sender, RoutedEventArgs e)
        {
            int selectedCustomerId = _showAllCustomerControl.GetSelectedCustomerId();
            if (selectedCustomerId != -1)
            {
                MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan törölni akarod a kiválasztott ügyfelet?");
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    DeleteCustomerAsync(selectedCustomerId);
                }
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Válassz ki egy ügyfelet a törléshez!");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private async void DeleteCustomerAsync(int customerId)
        {
            await _repository.DeleteCustomerAsync(customerId);
            Show_All_Customer();
        }

    }
}
