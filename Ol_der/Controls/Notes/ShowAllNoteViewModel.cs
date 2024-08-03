using Ol_der.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ol_der.Controls.Notes
{
    public class ShowAllNoteViewModel : INotifyPropertyChanged
    {
        private List<Note> _notes;
        private readonly NoteRepository _noteRepository;

        public List<Note> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public ShowAllNoteViewModel(int limit = 100)
        {
            _noteRepository = new NoteRepository();
            InitializeAsync(limit);
        }

        public async Task InitializeAsync(int limit)
        {
            Notes = await _noteRepository.GetAmountOfNotes(limit);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
