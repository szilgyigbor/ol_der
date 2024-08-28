using Ol_der.Controls.Orders;
using Ol_der.Controls.Warranties;
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

namespace Ol_der.Controls.CustomerOrders
{
    /// <summary>
    /// Interaction logic for CustomerOrderControl.xaml
    /// </summary>
    public partial class CustomerOrderControl : UserControl
    {
        private CustomerOrderRepository _customerOrderRepository;
        private ShowAllCustomerOrderControl _showAllCustomerOrderControl;

        public CustomerOrderControl()
        {
            InitializeComponent();
            ShowAllCustomerOrder();
            _customerOrderRepository = new();
        }

        public void ShowAllCustomerOrder(int customerOrderNumber = 100)
        {
            _showAllCustomerOrderControl = new ShowAllCustomerOrderControl(customerOrderNumber);
            ContentArea.Content = _showAllCustomerOrderControl;
        }

        private void Refresh()
        {
            ShowAllCustomerOrder();
        }

        private void AddCustomerOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxWindow MessageBox = new("Biztosan nyitunk új ügyfélrendelést?");
            if (MessageBox.ShowDialog() == false)
            {
                return;
            }

            AddOrUpdateCustomerOrderControl addOrUpdateCustomerOrderControl = new();
            addOrUpdateCustomerOrderControl.OnCustomerOrderFinished -= Refresh;
            addOrUpdateCustomerOrderControl.OnCustomerOrderFinished += Refresh;

            ContentArea.Content = addOrUpdateCustomerOrderControl;
        }

        private void UpdateCustomerOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder customerOrderToUpdate = _showAllCustomerOrderControl.GetSelectedCustomerOrder();

            if (customerOrderToUpdate == null)
            {
                MessageBoxWindow MessageBox = new("Nincs kiválasztott ügyfélrendelés!");
                MessageBox.ShowDialog();
                return;
            }

            MessageBoxOkWindow MessageBox1 = new("Ügyfélrendelés módosítása");
            MessageBox1.ShowDialog();


            AddOrUpdateCustomerOrderControl addOrUpdateCustomerOrderControl = new(customerOrderToUpdate);
            addOrUpdateCustomerOrderControl.OnCustomerOrderFinished -= Refresh;
            addOrUpdateCustomerOrderControl.OnCustomerOrderFinished += Refresh;

            ContentArea.Content = addOrUpdateCustomerOrderControl;
        }

        private void CustomerOrderDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder customerOrderToDetails = _showAllCustomerOrderControl.GetSelectedCustomerOrder();

            if (customerOrderToDetails == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztva garanciális ügy!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            CustomerOrderDetailControl customerOrderDetailControl = new(customerOrderToDetails);
            ContentArea.Content = customerOrderDetailControl;
        }

        private void DeleteCustomerOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder customerOrderToDelete = _showAllCustomerOrderControl.GetSelectedCustomerOrder();

            if (customerOrderToDelete == null)
            {
                MessageBoxWindow MessageBox = new("Nincs kiválasztott ügyfélrendelés!");
                MessageBox.ShowDialog();
                return;
            }

            MessageBoxWindow MessageBox1 = new("Biztosan törölni akarod az ügyfélrendelést?");
            if (MessageBox1.ShowDialog() == false)
            {
                return;
            }

            _customerOrderRepository.RemoveCustomerOrderAsync(customerOrderToDelete);
            Refresh();
        }
    }
}
