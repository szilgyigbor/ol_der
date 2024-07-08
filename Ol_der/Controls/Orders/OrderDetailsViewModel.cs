using OfficeOpenXml;
using OfficeOpenXml.Style;
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
        private Order _order;

        public Order Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveToTxtFileCommand { get; }
        public ICommand SaveToXlsxFileCommand { get; }

        public OrderDetailsViewModel(int orderId)
        {
            _orderRepository = new OrderRepository();
            LoadOrderAsync(orderId);
            SaveToTxtFileCommand = new RelayCommand(param => SaveToTxtFile());
            SaveToXlsxFileCommand = new RelayCommand(param => SaveToExcelFile());
        }

        private async Task LoadOrderAsync(int orderId)
        {
            Order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
        }

        private void SaveToTxtFile()
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, "orderItems.txt");

                var sb = new StringBuilder();
                sb.AppendLine("Darab".PadRight(8) + "Cikkszám".PadRight(12) + "Terméknév".PadRight(15) +
                    "( Azonosító szám: " + Order.OrderId + " )" );
                sb.AppendLine("----------------------------------------------------------------------------");

                foreach (var item in Order.OrderItems)
                {
                    sb.AppendLine(item.QuantityOrdered.ToString().PadRight(6) +
                                  item.Product.ItemNumber.PadRight(15) + "\t" +
                                  item.Product.Name + "\t");
                }

                File.WriteAllText(filePath, sb.ToString());

                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen elmentve a txt fájlba.");
                messageBoxOkWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Hiba történt a fájl mentése során: {ex.Message}");
                messageBoxOkWindow.ShowDialog();
            }
        }

        private void SaveToExcelFile()
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, "orderItems.xlsx");

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("OrderItems");

                    worksheet.Cells[1, 1].Value = "Darab";
                    worksheet.Cells[1, 2].Value = "Cikkszám";
                    worksheet.Cells[1, 3].Value = "Terméknév";

                    int row = 2;
                    foreach (var item in Order.OrderItems)
                    {
                        worksheet.Cells[row, 1].Value = item.QuantityOrdered;
                        worksheet.Cells[row, 2].Value = item.Product.ItemNumber;
                        worksheet.Cells[row, 3].Value = item.Product.Name;
                        row++;
                    }

                    using (var range = worksheet.Cells[1, 1, row - 1, 3])
                    {
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                }

                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen elmentve az xlsx fájlba.");
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
