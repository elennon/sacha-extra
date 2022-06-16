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
            
            database.DropTableAsync<Extra>().Wait();
            database.DropTableAsync<Pics>().Wait();
            database.CreateTableAsync<Extra>().Wait();
            database.CreateTableAsync<Pics>().Wait();
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
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public int SaveExtraAsync(Extra note)
        {
            if (note.ID != 0)
            {
                // Update an existing note.
                database.UpdateAsync(note);
            }
            else
            {
                // Save a new note.
                database.InsertAsync(note);
            }
            return note.ID;
        }

        public Task<int> DeleteExtraAsync(Extra note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }

        public Task<List<Pics>> GetPicsAsync(string extraId)
        {
            //Get all Quote.
            return database.Table<Pics>().Where(x => x.ExtraId == extraId).ToListAsync();
        }

        public Task<Pics> GetPicAsync(int id)
        {
            // Get a specific note.
            return database.Table<Pics>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePicAsync(Pics note)
        {
            if (note.ID != 0)
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

        public Task<int> DeletePicAsync(Pics note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
    }
}
