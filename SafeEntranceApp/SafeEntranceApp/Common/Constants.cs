using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafeEntranceApp.Common
{
    public class Constants
    {
        public const string WRONG_QR_MSG = "Código QR incorrecto";
        public const string WRONG_CODE_MSG = "Código de rastreo incorrecto";
        public const string WRONG_DATE_MSG = "Fecha incorrecta";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;
    }
}
