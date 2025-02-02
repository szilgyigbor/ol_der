using Ol_der.Models;
using System.Windows;
using System.Windows.Controls;


namespace Ol_der.Controls.Orders
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        private AddNewOrderControl _addOrderControl;
        private ShowAllOrderControl _showAllOrderControl;
        private GreenifyOrderControl _greenifyOrderControl;
        private OrderDetailsControl _orderDetailsControl;

        public int NumberToShow { get; set; } = 100;

        public OrderControl()
        {
            InitializeComponent();
            _showAllOrderControl = new ShowAllOrderControl(NumberToShow);
            ContentArea.Content = _showAllOrderControl;
        }

        public void Refresh()
        {
            _showAllOrderControl = new ShowAllOrderControl(NumberToShow);
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

            else return;
            
            OrderViewModel viewModel = new(NumberToShow);
            Order order = await viewModel.GetLastOpenOrderBySupplierIdAsync(supplierId);
           
            _addOrderControl = new AddNewOrderControl(order, supplierId);
            ContentArea.Content = _addOrderControl;
            _showAllOrderControl = new ShowAllOrderControl(NumberToShow);

            _addOrderControl.OnOrderFinished -= Refresh;
            _addOrderControl.OnOrderFinished += Refresh;
        }

        private async void ModifyOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllOrderControl;
            int orderId = _showAllOrderControl.GetSelectedOrderId();

            if (orderId != -1)
            {
                OrderViewModel viewModel = new(NumberToShow);
                Order order = await viewModel.GetOrderByOrderIdAsync(orderId);

                if (!order.IsOpen)
                {
                    MessageBoxWindow messageBoxWindow = new("A kiválasztott rendelés már le van zárva, biztosan módosítani akarod?");
                    messageBoxWindow.ShowDialog();

                    if (messageBoxWindow.DialogResult == true)
                    {
                        _addOrderControl = new AddNewOrderControl(order, order.SupplierId);
                        ContentArea.Content = _addOrderControl;

                        _addOrderControl.OnOrderFinished -= Refresh;
                        _addOrderControl.OnOrderFinished += Refresh;
                    }

                    return;
                }

                _addOrderControl = new AddNewOrderControl(order, order.SupplierId);
                ContentArea.Content = _addOrderControl;

                _addOrderControl.OnOrderFinished -= Refresh;
                _addOrderControl.OnOrderFinished += Refresh;
            }

            else
            {
                MessageBoxOkWindow messageBoxWindow1 = new("Válassz ki egy rendelést a módosításhoz!");
                messageBoxWindow1.ShowDialog();
            }
        }

        private async void GreenifyOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllOrderControl;
            int orderId = _showAllOrderControl.GetSelectedOrderId();

            if (orderId != -1)
            {
                OrderViewModel viewModel = new(NumberToShow);
                Order order = await viewModel.GetOrderByOrderIdAsync(orderId);

                if (order == null)
                {
                    MessageBoxOkWindow messageBoxWindow = new("Nem sikerült betölteni a rendelést (null)!");
                    messageBoxWindow.ShowDialog();
                    return;
                }

                if (order.IsOpen == true)
                {
                    MessageBoxOkWindow messageBoxWindow = new("A kiválasztott rendelés még nincs lezárva!");
                    messageBoxWindow.ShowDialog();
                    return;
                }

                if (order.IsColored == true)
                {
                    MessageBoxOkWindow messageBoxWindow = new("A rendelés már zöldítve lett! Így már nem lesz átemelve semmi!");
                    messageBoxWindow.ShowDialog();
                    _greenifyOrderControl = new GreenifyOrderControl(orderId);
                    ContentArea.Content = _greenifyOrderControl;
                    _greenifyOrderControl.OnGreened -= Refresh;
                    _greenifyOrderControl.OnGreened += Refresh;
                }

                _greenifyOrderControl = new GreenifyOrderControl(orderId);
                ContentArea.Content = _greenifyOrderControl;
                _greenifyOrderControl.OnGreened -= Refresh;
                _greenifyOrderControl.OnGreened += Refresh;
            }

            else
            {
                MessageBoxOkWindow messageBoxWindow1 = new("Válassz ki egy rendelést a zöldítéshez!");
                messageBoxWindow1.ShowDialog();
            }
        }

        private async Task ShowAllOrder(int limit)
        {
            _showAllOrderControl = new ShowAllOrderControl(limit);
            ContentArea.Content = _showAllOrderControl;
        }

        private async void ShowAllOrder_Click(object sender, RoutedEventArgs e)
        {
            InputOrderNumberWindow dialog = new InputOrderNumberWindow();
            if (dialog.ShowDialog() == true)
            {
                NumberToShow = dialog.NumberResult ?? 100;
                await ShowAllOrder(NumberToShow);
            }
        }

        private void ShowFilteredOrders_Click(object sender, RoutedEventArgs e)
        {
            _showAllOrderControl = new ShowAllOrderControl(NumberToShow);
            _showAllOrderControl.SearchOrdersByProductNumber();
            ContentArea.Content = _showAllOrderControl;
        }

        private async void ShowOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllOrderControl;
            int orderId = _showAllOrderControl.GetSelectedOrderId();

            if (orderId != -1)
            {
                OrderViewModel viewModel = new(NumberToShow);
                Order order = await viewModel.GetOrderByOrderIdAsync(orderId);

                if (order == null)
                {
                    MessageBoxOkWindow messageBoxWindow = new("Nem sikerült betölteni a rendelést");
                    messageBoxWindow.ShowDialog();
                    return;
                }

                _orderDetailsControl = new OrderDetailsControl(orderId);
                ContentArea.Content = _orderDetailsControl;
            }

            else
            {
                MessageBoxOkWindow messageBoxWindow1 = new("Válassz ki egy rendelést a részletekhez!");
                messageBoxWindow1.ShowDialog();
            }
        }

        private async void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _showAllOrderControl;
            int orderId = _showAllOrderControl.GetSelectedOrderId();

            if (orderId != -1)
            {

                MessageBoxWindow messageBoxWindow = new("Biztosan törölni akarod a kiválasztott rendelést?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    await _showAllOrderControl.DeleteOrderByOrderIdAsync(orderId);
                    Refresh();
                }
            }

            else
            {
                MessageBoxOkWindow messageBoxWindow1 = new("Válassz ki egy rendelést a törléshez!");
                messageBoxWindow1.ShowDialog();
            }


        }

        private async void RemoveDuplicates_Click(object sender, RoutedEventArgs e)
        {
            ExcelProcessor processor = new ExcelProcessor();
            processor.ProcessExcelFile();
        }
    }
}
