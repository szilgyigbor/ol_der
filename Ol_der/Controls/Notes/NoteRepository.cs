using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ol_der.Data;
using Ol_der.Models;
using Microsoft.EntityFrameworkCore;

namespace Ol_der.Controls.Notes
{
    internal class NoteRepository
    {
        public async Task<List<Note>> GetAllNoteAsync()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Notes.Where(n => !n.IsDeleted).ToListAsync();
            }
        }
    }
}
