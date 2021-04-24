using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();

        Task<T> GetById(int id);

        Task<int> Save(T element);
    }
}
