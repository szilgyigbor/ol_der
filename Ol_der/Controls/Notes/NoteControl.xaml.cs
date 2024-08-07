using Ol_der.Controls.Orders;
using Ol_der.Controls.Suppliers;
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
    /// Interaction logic for NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        private NoteViewModel _viewModel;
        private ShowAllNoteControl _showAllNoteControl;
        private AddNewNoteControl _addNewNoteControl;
        private NoteDetailControl _noteDetailControl;

        public NoteControl()
        {
            InitializeComponent();
            _viewModel = new NoteViewModel();
            DataContext = _viewModel;
            ShowAllNote();
        }

        private void ShowAllNote()
        {
            _showAllNoteControl = new ShowAllNoteControl();
            ContentArea.Content = _showAllNoteControl;
        }

        private void Show_All_Note_Click(object sender, RoutedEventArgs e)
        {
            ShowAllNote();
        }

        private void Add_Note_Click(object sender, RoutedEventArgs e)
        {
            _addNewNoteControl = new AddNewNoteControl(null);
            ContentArea.Content = _addNewNoteControl;
            _addNewNoteControl.OnNoteAdded -= ShowAllNote;
            _addNewNoteControl.OnNoteAdded += ShowAllNote;
        }

        private void Modify_Note_Click(object sender, RoutedEventArgs e)
        {
            Note SelectedNote = _showAllNoteControl.GetSelectedNote();

            if (SelectedNote == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztott jegyzet!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            else
            {
                _addNewNoteControl = new AddNewNoteControl(SelectedNote);
                ContentArea.Content = _addNewNoteControl;
                _addNewNoteControl.OnNoteAdded -= ShowAllNote;
                _addNewNoteControl.OnNoteAdded += ShowAllNote;
            }
        }

        private void Delete_Note_Click(object sender, RoutedEventArgs e)
        {
            Note SelectedNote = _showAllNoteControl.GetSelectedNote();

            if (SelectedNote == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztott jegyzet, mit törölnél!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            else
            {
                MessageBoxWindow messageBoxWindow = new($"Biztosan törölni akarod a {SelectedNote.NoteId}. jegyzetet?");
                messageBoxWindow.ShowDialog();
                if (messageBoxWindow.DialogResult == true)
                {
                    _viewModel.DeleteNote(SelectedNote);
                    ShowAllNote();
                }
            }
        }

        private void Note_Detail_Click(object sender, RoutedEventArgs e)
        {
            Note SelectedNote = _showAllNoteControl.GetSelectedNote();

            if (SelectedNote == null)
            {
                MessageBoxOkWindow messageBoxOkWindow = new("Nincs kiválasztott jegyzet a nagy nézethez!");
                messageBoxOkWindow.ShowDialog();
                return;
            }

            else
            {
                _noteDetailControl = new NoteDetailControl(SelectedNote);
                ContentArea.Content = _noteDetailControl;
            }
        }
    }
}
