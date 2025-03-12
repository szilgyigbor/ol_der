using Ol_der.Controls.Orders;
using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Notes
{
    public class ShowAllNoteViewModel : INotifyPropertyChanged
    {
        private readonly NoteRepository _noteRepository;
        private ObservableCollection<Note> _notes;
        private Note _selectedNote;

        public ObservableCollection<Note> Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (_selectedNote != value)
                {
                    _selectedNote = value;
                    OnPropertyChanged(nameof(SelectedNote));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ShowAllNoteViewModel(int limit)
        {
            _noteRepository = new NoteRepository();
            InitializeAsync(limit);
        }

        public async Task InitializeAsync(int limit)
        {
            var notes = await _noteRepository.GetAmountOfNotes(limit);
            Notes = new ObservableCollection<Note>(notes); 
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
