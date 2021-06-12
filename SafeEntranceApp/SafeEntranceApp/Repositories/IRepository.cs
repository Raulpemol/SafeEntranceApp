using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeEntranceApp.Repositories
{
    public interface IRepository<T>
    {
        /*
         * Devuelve una lista con todos los elementos de la tabla
         */
        Task<List<T>> GetAll();

        /*
         * Devuelve el elemento de la tabla con un identificador concreto
         */
        Task<T> GetById(int id);

        /*
         * Guarda un elemento en la tabla de la base de datos. Si el elemento ya existe, lo actualiza
         */
        Task<int> Save(T element);
    }
}
