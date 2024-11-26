using Ol_der.Models;
using Ol_der.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using Ol_der.Controls.Orders;
using System.IO;

namespace Ol_der.Controls.Sales
{
    /// <summary>
    /// Interaction logic for ShowAllSaleControl.xaml
    /// </summary>
    public partial class ShowAllSaleControl : UserControl
    {
        private SaleViewModel _viewModel;
        public List<Sale> Sales { get; set; }
        public ShowAllSaleControl()
        {
            InitializeComponent();
            _viewModel = new SaleViewModel();
            this.DataContext = _viewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Parent is StackPanel panel)
            {
                var textBlock = panel.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    string textToSave = textBlock.Text;

                    SaveText(textToSave);
                }
            }
        }

        private void SaveText(string text)
        {
            try
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = System.IO.Path.Combine(desktopPath, "daySummary.txt");

                var sb = new StringBuilder();
                sb.AppendLine("------------------------------------------------------------------" +
                    "-----------------------------------------------------------");
                sb.AppendLine(text);


                File.AppendAllText(filePath, sb.ToString());

                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Sikeresen hozzáadva a daySummary.txt fájlhoz.");
                messageBoxOkWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Hiba történt a fájl mentése során: {ex.Message}");
                messageBoxOkWindow.ShowDialog();
            }
        }


        public int SaleIdToModify()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                return SelectedSale.SaleId;
            }

            return -1;
        }

        public async Task RefreshSales(object filterForSales)
        {
            await _viewModel.RefreshData(filterForSales);
        }

        public async Task DeleteSale()
        {
            if (SalesListView.SelectedItem is Sale SelectedSale)
            {
                MessageBoxWindow messageBoxWindow = new MessageBoxWindow("Biztosan törölni szeretnéd ezt az eladást?");
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    await _viewModel.DeleteSaleAsync(SelectedSale);
                }
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow("Válassz ki egy eladást a törléshez!");
            }

        }

        public async Task LoadSearchedSales(string productNumber)
        {
            await _viewModel.LoadSearchedSalesAsync(productNumber);
        }

    }
}
