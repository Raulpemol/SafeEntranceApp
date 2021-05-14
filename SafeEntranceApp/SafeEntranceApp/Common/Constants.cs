using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafeEntranceApp.Common
{
    public class Constants
    {
        public const string APP_NAME = "SafeEntrance";

        public const string WRONG_QR_MSG = "Código QR incorrecto";
        public const string WRONG_CODE_MSG = "Código de rastreo incorrecto";
        public const string WRONG_DATE_MSG = "Fecha incorrecta";
        public const string SERVER_ERROR = "Error al conectarse al servidor";
        public const string ALERT_REGISTERED = "Alerta registrada, ¡cuídese!";
        public const string SYNC_FREQUENCY_MSG = "Seleccione la frecuencia de actualización deseada";

        public const string ERROR_ICON = "alert";
        public const string CORRECT_ICON = "checked";
        public const string DOOR_OPEN = "door_open";
        public const string DOOR_CLOSED = "door_closed";

        public const string TITLE_KEY = "title";
        public const string MESSAGE_KEY = "message";
        public const string NOTIFICATION_TITLE = "Sincronización completada";
        public const string NOTIFICATION_NO_ALERTS_MSG = "No hay nuevas alertas que te afecten";
        public const string NOTIFICATION_ALERTS_MSG = "Cuidado, hay nuevas alertas";

        public static readonly int[] SYNC_FREQUENCIES = new int[4] { 1, 5, 12, 24 };

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public const string ALERTS_HELP_TEXT = "Las alertas aquí mostradas provienen de coincidencias temporales en un mismo local con una persona que ha dado positivo en COVID-19.\n\n" +
            "El tiempo y la distancia a la que se produjo el contacto para ser considerado alerta puede variar en base a los nuevos descubrimientos y parámetros establecidos por los organismos internacionales.\n\n" +
            "En caso de que que se muestre alguna alerta nueva, por favor, póngase en contacto con su centro de salud habitual.";
    }
}
