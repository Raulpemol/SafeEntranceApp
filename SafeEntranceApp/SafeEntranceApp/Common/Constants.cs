using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafeEntranceApp.Common
{
    public class Constants
    {
        public const string DATABASE_FILENAME = "safeentrancelocaldb.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(basePath, DATABASE_FILENAME);
            }
        }
    }
}
