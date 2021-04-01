using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeEntranceApp.Repositories
{
    class VisitsRepository : IRepository<Visit>
    {
        private readonly ISQLPlatform platform = DependencyService.Get<ISQLPlatform>();
        private SQLiteAsyncConnection database;

        public VisitsRepository()
        {
            database = platform.GetConnectionAsync();
            database.CreateTableAsync<Visit>();
        }

        public Task<List<Visit>> GetAll()
        {
            return database.Table<Visit>().ToListAsync();
        }

        public Task<List<Visit>> GetSelfInfected(DateTime date)
        {
            return database.Table<Visit>().Where(v => v.EnterDateTime >= date || v.ExitDateTime >= date).ToListAsync();
        }

        public Task<Visit> GetById(int id)
        {
            return database.Table<Visit>().Where(v => v.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> Save(Visit visit)
        {
            if (visit.ID != 0)
            {
                return database.UpdateAsync(visit);
            }
            else
            {
                return database.InsertAsync(visit);
            }
        }
    }
}
