using SafeEntranceApp.Models;
using SafeEntranceApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Services.Database
{
    public class CovidAlertsService : IDatabaseService<CovidAlert>
    {
        private CovidAlertsRepository repository;

        public CovidAlertsService()
        {
            repository = new CovidAlertsRepository();
        }

        public async Task<List<CovidAlert>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<CovidAlert> GetById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<int> Save(CovidAlert alert)
        {
            return await repository.Save(alert);
        }
    }
}
