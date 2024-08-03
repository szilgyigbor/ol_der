using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ol_der.Controls.Notes
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowAllNotesCommand { get; }
        public ICommand AddNoteCommand { get; }
        public ICommand ModifyNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }

        public NoteViewModel()
        {
            ShowAllNotesCommand = new RelayCommand(ShowAllNotes);
            AddNoteCommand = new RelayCommand(AddNote);
            ModifyNoteCommand = new RelayCommand(ModifyNote);
            DeleteNoteCommand = new RelayCommand(DeleteNote);

            CurrentViewModel = new ShowAllNoteViewModel();
        }

        private void ShowAllNotes()
        {
            CurrentViewModel = new ShowAllNoteViewModel();
        }

        private void AddNote()
        {
            CurrentViewModel = new AddNewNoteViewModel();
        }

        private void ModifyNote()
        {
            // Logika a jegyzet módosítására
        }

        private void DeleteNote()
        {
            // Logika a jegyzet törlésére
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
