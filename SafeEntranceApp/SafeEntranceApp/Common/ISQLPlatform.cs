using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Common
{
    public interface ISQLPlatform
    {
        /*
         * Obtiene una conexión con la base de datos SQL del dispositivo
         */
        SQLiteConnection GetConnection();

        /*
         * Obtiene una conexión asíncrona con la base de datos SQL del dispositivo
         */
        SQLiteAsyncConnection GetConnectionAsync();
    }
}
