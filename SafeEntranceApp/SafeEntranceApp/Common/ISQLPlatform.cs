using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Common
{
    public interface ISQLPlatform
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetConnectionAsync();
    }
}
