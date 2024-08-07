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
        public async Task<List<Note>> GetAmountOfNotes(int limit)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                return await context.Notes
                    .OrderByDescending(n => n.NoteId)
                    .Take(limit)
                    .ToListAsync();
            }
        }

        public async Task AddNewNote (Note note)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Notes.Add(note);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateNote (Note note)
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Notes.Update(note);
                await context.SaveChangesAsync();
            }
        }
    }
}
