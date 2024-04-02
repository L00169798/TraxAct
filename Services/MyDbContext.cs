using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraxAct.Models;
using System.Diagnostics;

namespace TraxAct.Services
{
    public class MyDbContext
    {
        const string TraxActDB = "TraxActDB.db3";
        static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, TraxActDB);
        const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        SQLiteAsyncConnection Database;

        async Task Init()
        {
            if (Database != null)
            {
                return;
            }

            var databaseFileExists = File.Exists(DatabasePath);
            if (!databaseFileExists)
            {
                Debug.WriteLine($"Database file does not exist at path: {DatabasePath}");
                Database = new SQLiteAsyncConnection(DatabasePath, Flags);
                await Database.CreateTableAsync<Event>();
                Debug.WriteLine($"Database created at path: {DatabasePath}");
            }
            else
            {
                Debug.WriteLine($"Database file exists at path: {DatabasePath}");
                Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            }
        }
        public async Task<List<Event>> GetEvents()
        {
            await Init();
            return await Database.Table<Event>().ToListAsync();
        }

        public async Task<Event> GetById(int id)
        {
            await Init();
            return await Database.Table<Event>().Where(x => x.EventId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Create(Event newEvent)
        {
            try
            {
                await Init();

                if (Database == null)
                {
                    Console.WriteLine("Error: Database is null.");
                    return false;
                }

                await Database.InsertAsync(newEvent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving event: {ex.Message}");

                Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine($"Inner Exception: {innerException.Message}");
                    innerException = innerException.InnerException;
                }

                if (ex.Message.Contains("format"))
                {
                    Console.WriteLine("Error: Data format issue occurred.");
                }
                else
                {
                    Console.WriteLine("Error: An unexpected error occurred.");
                }

                return false;
            }
        }


        public async Task Update(Event updatedEvent)
        {
            await Init();
            await Database.UpdateAsync(updatedEvent);
        }

        public async Task Delete(Event existingEvent)
        {
            await Init();
            await Database.DeleteAsync(existingEvent);
        }

        public async Task<List<Event>> GetEventsByStartDateAsync(DateTime startDate)
        {
            await Init();
            var events = await Database.Table<Event>().ToListAsync();
            return events.FindAll(e => e.StartTime >= startDate);
        }


        public async Task<List<Event>> GetEventsInRange(DateTime startDate, DateTime endDate)
        {
            await Init();
            var events = await Database.Table<Event>().ToListAsync();
            return events.FindAll(e => e.StartTime >= startDate && e.StartTime <= endDate);
        }
    }
}
