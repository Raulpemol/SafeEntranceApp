using SafeEntranceApp.Models;
using SafeEntranceApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Database
{
    class VisitsService : IDatabaseService<Visit>
    {
        private VisitsRepository repository;

        public VisitsService()
        {
            repository = new VisitsRepository();
        }

        public async Task<List<Visit>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<List<Visit>> GetSelfInfected(DateTime date)
        {
            return await repository.GetSelfInfected(date);
        }

        public async Task<Visit> GetById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<int> Save(Visit visit)
        {
            return await repository.Save(visit);
        }
    }
}
