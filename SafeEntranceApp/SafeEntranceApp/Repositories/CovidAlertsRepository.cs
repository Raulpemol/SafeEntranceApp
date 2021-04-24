using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeEntranceApp.Repositories
{
    public class CovidAlertsRepository : IRepository<CovidAlert>
    {
        private readonly ISQLPlatform platform = DependencyService.Get<ISQLPlatform>();
        private SQLiteAsyncConnection database;

        public CovidAlertsRepository()
        {
            database = platform.GetConnectionAsync();
            database.CreateTableAsync<CovidAlert>();
        }

        public Task<List<CovidAlert>> GetAll()
        {
            return database.Table<CovidAlert>().ToListAsync();
        }

        public Task<CovidAlert> GetById(int id)
        {
            return database.Table<CovidAlert>().Where(v => v.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> Save(CovidAlert alert)
        {
            if (alert.ID != 0)
            {
                return database.UpdateAsync(alert);
            }
            else
            {
                return database.InsertAsync(alert);
            }
        }
    }
}
