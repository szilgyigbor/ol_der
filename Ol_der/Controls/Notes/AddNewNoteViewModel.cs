using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Ol_der.Models;
using Ol_der.Controls.Orders;
using Ol_der.Controls.Notes;

namespace Ol_der.Controls.Notes
{
    public class AddNewNoteViewModel : INotifyPropertyChanged
    {
        private readonly NoteRepository _noteRepository;

        private bool _isNewNote;
        private Note _note;
        public Note Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveNoteCommand { get; }

        public AddNewNoteViewModel(Note noteToUpdate)
        {
            _noteRepository = new NoteRepository();
            SaveNoteCommand = new RelayCommand(async () => await SaveNote());
            CheckNote(noteToUpdate);
        }

        public void CheckNote(Note noteToUpdate)
        {
            if (noteToUpdate == null)
            {
                Note = new Note();
                _isNewNote = true;
            }

            else
            {
                Note = noteToUpdate;
                _isNewNote = false;
            }
        }

        public async Task SaveNote()
        {
            if (_isNewNote)
            {
                Note.CreationDate = DateTime.Now;

                await _noteRepository.AddNewNote(Note);

                MessageBoxOkWindow messageBoxWindow = new("Jegyzet sikeresen mentve");
                messageBoxWindow.ShowDialog();
            }
            else 
            {
                await _noteRepository.UpdateNote(Note);

                MessageBoxOkWindow messageBoxWindow = new("Jegyzet sikeresen módosítva");
                messageBoxWindow.ShowDialog();
            }
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}