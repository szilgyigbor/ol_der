using Ol_der.Controls.Orders;
using Ol_der.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Ol_der.Controls.SalePackages
{
    /// <summary>
    /// Interaction logic for ShowAllPackageControl.xaml
    /// </summary>
    public partial class ShowAllPackageControl : UserControl
    {
        private ShowAllPackageViewModel _viewModel;
        public List<Sale> Sales { get; set; }
        public ShowAllPackageControl()
        {
            InitializeComponent();
            _viewModel = new ShowAllPackageViewModel();
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
                MessageBoxWindow messageBoxWindow = new("Biztosan törölni szeretnéd ezt az csomagot?");
                messageBoxWindow.ShowDialog();

                if (messageBoxWindow.DialogResult == true)
                {
                    await _viewModel.DeleteSaleAsync(SelectedSale);
                }
            }
            else
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Válassz ki egy csomagot a törléshez!");
                messageBoxOkWindow.ShowDialog();
            }

        }

        public async Task LoadSearchedSales(Dictionary<string, string> SearchCriteria)
        {
            await _viewModel.LoadSearchedSalesAsync(SearchCriteria);
        }

    }
}
