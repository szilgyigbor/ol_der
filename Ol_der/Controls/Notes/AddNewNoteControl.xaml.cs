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

using Ol_der.Models;

namespace Ol_der.Controls.Notes
{
    /// <summary>
    /// Interaction logic for AddNewNoteControl.xaml
    /// </summary>
    public partial class AddNewNoteControl : UserControl
    {
        private AddNewNoteViewModel _viewModel;

        public AddNewNoteControl(Note NoteToUpdate)
        {
            InitializeComponent();
            _viewModel = new AddNewNoteViewModel(NoteToUpdate);
            DataContext = _viewModel;
        }
    }
}
