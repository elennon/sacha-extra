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

            //database.DropTableAsync<Extra>().Wait();
            //database.DropTableAsync<Pics>().Wait();
            //database.DropTableAsync<Project>().Wait();
            //database.DropTableAsync<Batch>().Wait();

            database.CreateTableAsync<Extra>().Wait();
            database.CreateTableAsync<Pics>().Wait();
            database.CreateTableAsync<Project>().Wait();
            database.CreateTableAsync<Batch>().Wait();
        }

        public Task<List<Extra>> GetExtrasAsync(string prjId)
        {
            //Get all Quote.
            return database.Table<Extra>().Where(x => x.ProjectId == prjId).ToListAsync();
        }

        public Task<Extra> GetExtraAsync(string myid)
        {
            // Get a specific note.
            return database.Table<Extra>()
                            .Where(i => i.MyId == myid)
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
        public Task<List<Project>> GetProjectsAsync()
        {
            //Get all Quote.
            return database.Table<Project>().ToListAsync();
        }

        public Task<Project> GetProjectAsync(string myid)
        {
            // Get a specific note.
            return database.Table<Project>()
                            .Where(i => i.MyId == myid)
                            .FirstOrDefaultAsync();
        }

        public Task<Project> GetCurrentProjectAsync()
        {
            // Get a specific note.
            return database.Table<Project>()
                            .Where(i => i.IsCurrent == true)
                            .FirstOrDefaultAsync();
        }

        public int SaveProjectAsync(Project note)
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

        public Task<int> DeleteProjectAsync(Project note)
        {
            // Delete a note.
            return database.DeleteAsync(note);
        }
        public Task<Batch> GetBatchAsync(string myid)
        {
            // Get a specific note.
            return database.Table<Batch>()
                            .Where(i => i.BatchId == myid)
                            .FirstOrDefaultAsync();
        } 
        public Task<List<Extra>> GetBatchExtrasAsync(Batch bt)
        {
            // Get a specific note.
            return database.Table<Extra>()
                            .Where(i => i.BatchId == bt.BatchId)
                            .ToListAsync();
        }
        public Task<List<Batch>> GetBatchesAsync()
        {
            return database.Table<Batch>().ToListAsync();
        }
        public int SaveBatchAsync(Batch note)
        {
            if (note.ID != 0)
            {
                database.UpdateAsync(note);
            }
            else
            {
                database.InsertAsync(note);
            }
            return note.ID;
        }

    }
}
