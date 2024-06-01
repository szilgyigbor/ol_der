using Ol_der.Controls.Sales;
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

namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        private AddNewOrderControl _addOrderControl;
        private ShowAllOrderControl _showAllOrderControl;

        public OrderControl()
        {
            InitializeComponent();
            _showAllOrderControl = new ShowAllOrderControl();
            ContentArea.Content = _showAllOrderControl;
        }

        private async void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            int supplierId = -1;

            SelectSupplierWindow dialog = new();
            if (dialog.ShowDialog() == true)
            {
                supplierId = dialog.supplierId;
            }
            
            OrderViewModel viewModel = new();
            Order order = await viewModel.GetLastOpenOrderBySupplierIdAsync(supplierId);
           
            _addOrderControl = new AddNewOrderControl(order, supplierId);
            ContentArea.Content = _addOrderControl;
        }

        private async void ModifyOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllOrderControl;
            int orderId = _showAllOrderControl.OrderIdToModify();

            if (orderId != -1)
            {
                OrderViewModel viewModel = new();
                Order order = await viewModel.GetLastOpenOrderByOrderIdIdAsync(orderId);

                if (order == null)
                {
                    MessageBoxWindow messageBoxWindow = new("A kiválasztott rendelés már le van zárva!");
                    messageBoxWindow.ShowDialog();
                    return;
                }

                _addOrderControl = new AddNewOrderControl(order, order.SupplierId);
                ContentArea.Content = _addOrderControl;
            }

            else
            {
                MessageBoxWindow messageBoxWindow1 = new("Válassz ki egy rendelést a módosításhoz!");
                messageBoxWindow1.ShowDialog();
            }
        }

        private async Task ShowAllOrder(int limit = 100)
        {
            _showAllOrderControl = new ShowAllOrderControl();
            ContentArea.Content = _showAllOrderControl;
        }

        private async void ShowAllOrder_Click(object sender, RoutedEventArgs e)
        {
            ShowAllOrder();
        }

        private async void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            /*await _showAllSaleControl.DeleteSale();
            await ShowAllSale();*/
        }

        private async void SearchOrdersByProductNumber_Click(object sender, RoutedEventArgs e)
        {
            /*InputProductNumberWindow dialog = new InputProductNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                string productNumber = dialog.ProductNumber;
                await ShowSalesByProductNumber(productNumber);
            }*/
        }
    }
}
