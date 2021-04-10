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
    class CovidContactRepository : IRepository<CovidContact>
    {
        private readonly ISQLPlatform platform = DependencyService.Get<ISQLPlatform>();
        private SQLiteAsyncConnection database;

        public CovidContactRepository() 
        {
            database = platform.GetConnectionAsync();
            database.CreateTableAsync<CovidContact>();
        }

        public Task<List<CovidContact>> GetAll()
        {
            return database.Table<CovidContact>().ToListAsync();
        }

        public Task<CovidContact> GetById(int id)
        {
            return database.Table<CovidContact>().Where(v => v.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> Save(CovidContact contact)
        {
            if (contact.ID != 0)
            {
                return database.UpdateAsync(contact);
            }
            else
            {
                return database.InsertAsync(contact);
            }
        }
    }
}
