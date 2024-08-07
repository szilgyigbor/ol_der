using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ol_der.Controls.Orders;
using Ol_der.Models;

namespace Ol_der.Controls.Notes
{
    public class NoteViewModel
    {
        private NoteRepository _noteRepository;

        public NoteViewModel()
        {
            _noteRepository = new NoteRepository();
        }

        public Task DeleteNote(Note NoteToRemove) 
        {
            return _noteRepository.DeleteNote(NoteToRemove);
        }
    }
}
