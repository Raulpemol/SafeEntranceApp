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
        public const string SERVER_ERROR = "Error al conectarse al servidor";
        public const string ALERT_REGISTERED = "Alerta registrada, ¡cuídese!";
        public const string SYNC_FREQUENCY_MSG = "Seleccione la frecuencia de actualización deseada";

        public const string ERROR_ICON = "alert.png";
        public const string CORRECT_ICON = "checked.png";

        public const string TITLE_KEY = "title";
        public const string MESSAGE_KEY = "message";

        public static readonly int[] SYNC_FREQUENCIES = new int[4] { 1, 5, 12, 24 };

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;
    }
}
