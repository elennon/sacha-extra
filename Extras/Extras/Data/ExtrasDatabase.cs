using SQLite;
using Extras.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Extras.Data
{
    public class ExtrasDatabase
    {
        readonly SQLiteAsyncConnection database;

        public ExtrasDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Extra>().Wait();
        }

        public Task<List<Extra>> GetExtrasAsync()
        {
            //Get all Quote.
            return database.Table<Extra>().ToListAsync();
        }

        public Task<Extra> GetExtraAsync(int id)
        {
            // Get a specific note.
            return database.Table<Extra>()
                            .Where(i => i.id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveExtraAsync(Extra note)
        {
            if (note.id != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(note);
            }
        }

        public Task<int> DeleteExtraAsync(Extra note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }

    }
}
