using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ol_der.Controls.Orders
{
    internal class OrderDetailsViewModel : INotifyPropertyChanged
    {
        private OrderRepository _orderRepository;

        public Order Order { get; set; }
        public ICommand SaveToFileCommand { get; }

        public OrderDetailsViewModel(int orderId)
        {
            _orderRepository = new OrderRepository();
            LoadOrderAsync(orderId);
            SaveToFileCommand = new RelayCommand(param => SaveToFile());
        }

        private async Task LoadOrderAsync(int orderId)
        {
            Order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
        }

        private void SaveToFile()
        {
            try
            {
                var filePath = "orderItems.txt";
                var sb = new StringBuilder();
                sb.AppendLine("Darab".PadRight(8) + "Cikkszám".PadRight(12) + "Terméknév".PadRight(20));
                sb.AppendLine("----------------------------------------------------------------------------");

                foreach (var item in Order.OrderItems)
                {
                    sb.AppendLine(item.QuantityOrdered.ToString().PadRight(6) +
                                  item.Product.ItemNumber.PadRight(15) + "\t" +
                                  item.Product.Name + "\t");
                }

                File.WriteAllText(filePath, sb.ToString());

                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen elmentve a fájlba.");
                messageBoxOkWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Hiba történt a fájl mentése során: {ex.Message}");
                messageBoxOkWindow.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
