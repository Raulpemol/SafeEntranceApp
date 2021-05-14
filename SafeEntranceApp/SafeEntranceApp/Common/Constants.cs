using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafeEntranceApp.Common
{
    public class Constants
    {
        #region Global
        public const string APP_NAME = "SafeEntrance";
        #endregion

        #region Scanner
        public const string WRONG_QR_MSG = "Código QR incorrecto";
        public const string ENTER_PLACE_ACTION = "Entrar a un local";
        public const string EXIT_PLACE_ACTION = "Salir del local";
        #endregion

        #region CreateAlert
        public const string WRONG_CODE_MSG = "Código de rastreo incorrecto";
        public const string WRONG_DATE_MSG = "Fecha incorrecta";
        public const string SERVER_ERROR = "Error al conectarse al servidor";
        public const string ALERT_REGISTERED = "Alerta registrada, ¡cuídese!";
        #endregion

        #region ShowAlerts
        public const string SYNC_FREQUENCY_MSG = "Seleccione la frecuencia de actualización deseada";
        public static readonly int[] SYNC_FREQUENCIES = new int[4] { 1, 5, 12, 24 };
        public static readonly string[] SYNC_OPTIONS_TEXT = new string[4] { "1 hora", "5 horas", "12 horas", "1 día" };
        #endregion

        #region ImageResources
        public const string ERROR_ICON = "alert";
        public const string CORRECT_ICON = "checked";
        public const string DOOR_OPEN = "door_open";
        public const string DOOR_CLOSED = "door_closed";
        #endregion

        #region Notifications
        public const string TITLE_KEY = "title";
        public const string MESSAGE_KEY = "message";
        public const string NOTIFICATION_TITLE = "Sincronización completada";
        public const string NOTIFICATION_NO_ALERTS_MSG = "No hay nuevas alertas que te afecten";
        public const string NOTIFICATION_ALERTS_MSG = "Cuidado, hay nuevas alertas";

        public const string NOTIFICATION_CHANNEL_ID = "safeentrancechannel";
        public const string NOTIFICATION_CHANNEL_NAME = "SafeEntranceChannel";
        public const string NOTIFICATION_CHANNEL_DESCRIPTION = "Channel for showing alert notifications";

        public const string NOTIFICATION_SERVICE_NAME = "com.uniovi.safeentranceapp.fetch";
        #endregion

        #region RestServices
        public const string REST_POST = "POST";
        public const string JSON_FORMAT = "application/json";
        #endregion

        #region ColorResources
        public const string RESOURCE_ACCENT = "Accent";
        public const string RESOURCE_ALTERNATIVE = "Alternative";
        public const string RESOURCE_SECONDARY_ACCENT = "SecondaryAccent";
        public const string RESOURCE_TEXT_BLACK = "TextColorBlack";
        #endregion

        #region SharedPreferences
        public const string SYNC_PERIOD_PREFERENCE = "sync_period";
        public const string LAST_SYNC_PREFERENCE = "last_sync";
        public const string NEXT_SYNC_PREFERENCE = "next_sync";
        public const string AUTO_SYNC_PREFERENCE = "auto_sync";
        public const string USER_STATE_PREFERENCE = "user_state";
        public const string CURRENT_VISIT_PREFERENCE = "current_visit";
        #endregion

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public const string ALERTS_HELP_TEXT = "Las alertas aquí mostradas provienen de coincidencias temporales en un mismo local con una persona que ha dado positivo en COVID-19.\n\n" +
            "El tiempo y la distancia a la que se produjo el contacto para ser considerado alerta puede variar en base a los nuevos descubrimientos y parámetros establecidos por los organismos internacionales.\n\n" +
            "En caso de que que se muestre alguna alerta nueva, por favor, póngase en contacto con su centro de salud habitual.";
    }
}
