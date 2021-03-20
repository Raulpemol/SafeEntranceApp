using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Repositories
{
    class VisitsRepository : IRepository<Visit>
    {
        private static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<VisitsRepository> Instance = new AsyncLazy<VisitsRepository>(async () =>
        {
            var instance = new VisitsRepository();
            CreateTableResult result = await Database.CreateTableAsync<Visit>();
            return instance;
        });

        public VisitsRepository()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<Visit>> GetAll()
        {
            return Database.Table<Visit>().ToListAsync();
        }

        public Task<Visit> GetById(int id)
        {
            return Database.Table<Visit>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> Save(Visit visit)
        {
            if (visit.ID != 0)
            {
                return Database.UpdateAsync(visit);
            }
            else
            {
                return Database.InsertAsync(visit);
            }
        }
    }
}
