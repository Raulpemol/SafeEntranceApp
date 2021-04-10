using SafeEntranceApp.Models;
using SafeEntranceApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Database
{
    class CovidContactService : IDatabaseService<CovidContact>
    {
        private CovidContactRepository repository;

        public CovidContactService()
        {
            repository = new CovidContactRepository();
        }

        public async Task<List<CovidContact>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<CovidContact> GetById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<int> Save(CovidContact contact)
        {
            return await repository.Save(contact);
        }

        public int SaveAll(List<CovidContact> contacts)
        {
            int added = 0;

            Task.Run(() => contacts.ForEach(async c => added += await repository.Save(c)));

            return added;
        }
    }
}
