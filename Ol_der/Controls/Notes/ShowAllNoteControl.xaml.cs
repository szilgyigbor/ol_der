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

namespace Ol_der.Controls.Notes
{
    /// <summary>
    /// Interaction logic for ShowAllNoteControl.xaml
    /// </summary>
    public partial class ShowAllNoteControl : UserControl
    {
        private ShowAllNoteViewModel _viewModel;
        public ShowAllNoteControl()
        {
            InitializeComponent();
            _viewModel = new ShowAllNoteViewModel();
            this.DataContext = _viewModel;
        }

        public Note GetSelectedNote()
        {
            return _viewModel.SelectedNote;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            if (scrollViewer != null)
            {
                double scrollAmount = e.Delta > 0 ? -50.0 : 50.0; 
                double newOffset = scrollViewer.VerticalOffset + scrollAmount;

                scrollViewer.ScrollToVerticalOffset(newOffset);

                e.Handled = true;
            }
        }
    }
}
