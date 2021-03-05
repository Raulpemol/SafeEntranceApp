using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services
{
    class DatabaseManager
    {
        private static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<DatabaseManager> Instance = new AsyncLazy<DatabaseManager>(async () =>
        {
            var instance = new DatabaseManager();
            CreateTableResult result = await Database.CreateTableAsync<DatabaseManager>();
            return instance;
        });

        public DatabaseManager()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Visit>> GetVisitsAsync()
        {
            return Database.Table<Visit>().ToListAsync();
        }

        public Task<Visit> GetVisitAsync(int id)
        {
            return Database.Table<Visit>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
    }
}
