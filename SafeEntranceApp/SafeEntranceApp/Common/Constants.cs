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
        public const string PLACE_FULL_MSG = "Acceso denegado, el local está lleno";
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
        public const string IS_FIRST_START = "first_start";
        #endregion

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public const string ALERTS_HELP_TEXT = "Las alertas aquí mostradas provienen de coincidencias temporales en un mismo local con una persona que ha dado positivo en COVID-19.\n\n" +
            "El tiempo y la distancia a la que se produjo el contacto para ser considerado alerta puede variar en base a los nuevos descubrimientos y parámetros establecidos por los organismos internacionales.\n\n" +
            "En caso de que que se muestre alguna alerta nueva, por favor, póngase en contacto con su centro de salud habitual.";

        public const string TERMS_AND_CONDITIONS = "Objetivo y finalidad de la aplicación:" +
            "\nEsta aplicación ha sido desarrollada por Raúl Pérez Molinero, como parte de su proyecto de fin de grado para la Universidad de Oviedo.\nEl objetivo de esta aplicación es permitir el rastreo automático de contagios de COVID-19 producidos en locales de cualquier tipo." +
            "\n\nDatos del responsable del tratamiento de datos:\n" +
            "- Nombre completo: Raúl Pérez Molinero\n" +
            "- Domicilio: Calle Miguel de Unamuno, Nº8, CP 33010, Oviedo, Asturias, España\n" +
            "- Teléfono de contacto: 634566609\n" +
            "- Correo electrónico: UO263743 @uniovi.es\n\n" +
            "Tratamiento de datos y funcionamiento:\n" +
            "SafeEntrance no recoge datos personales de ningún tipo y todos los procesos de acceso a locales y creación y obtención de alertas se realizan de manera anónima.\n" +
            "Por cada acceso a un local, tras escanear el código QR correspondiente, la aplicación almacena en la base de datos local del dispositivo el identificador del local y la hora de acceso y salida del mismo.\n" +
            "A la hora de notificar un positivo por coronavirus, se solicita la fecha de aparición de los síntomas o realización de la prueba PCR y el código de rastreo proporcionado por las autoridades sanitarias competentes. Dicha alerta será almacenada localmente y en remoto con el fin de ser notificada a los usuarios que puedan estar afectados.Los datos tratados para notificar una alerta son los siguientes:\n" +
            "- Fecha de aparición de síntomas o realización de la prueba PCR.\n" +
            "- Código sanitario.\n" +
            "- Visitas a locales realizadas en los periodos de infectividad marcados por las autoridades sanitarias. Estas visitas contienen el identificador del local y las fechas y horas de entrada y salida.\n" +
            "Para obtener las alertas que puedan afectar al usuario, la aplicación de SafeEntrance no transmite al servidor datos personales, comunicando únicamente los identificadores de los locales visitados en los plazos de incubación marcados por las autoridades sanitarias, la fecha de la última sincronización realizada en este dispositivo y la lista de identificadores de alertas creadas desde este dispositivo y todo el procesamiento posterior se realiza en el propio dispositivo móvil.\n\n" +
            "Legitimidad del tratamiento de datos:\n" +
            "El tratamiento de los datos se basa en el consentimiento por parte del usuario expresado en el momento de aceptar estos términos y condiciones.\n\n" +
            "Propiedad intelectual y derechos de autor:\n" +
            "El código fuente, los diseños gráficos, iconos, animaciones, textos, así como la información y, en definitiva, los elementos contenidos en la página web están protegidos por la legislación española sobre los derechos de propiedad intelectual e industrial a favor de la Universidad de Oviedo. El usuario podrá utilizarlos para su uso personal, quedando prohibida la utilización con fines comerciales de los mismos. No está permitida la reproducción y/o publicación (total o parcial), ni su tratamiento informático, distribución, difusión, modificación, transformación o descompilación, sin el permiso previo y por escrito del titular.En caso de que se autorice la reproducción, se ha de indicar la procedencia de la información.\n" +
            "Los iconos que aparecen en la aplicación fueron hechos por Icons8 y Freepik (www.freepik.com) de Flaticon (www.flaticon.com).\n\n" +
            "Derechos del interesado:\n" +
            "Cualquier afectado por el tratamiento de datos realizado en SafeEntrance tiene derecho a obtener información acerca del tratamiento de datos personales que le conciernen.\n\n" +
            "Derecho a presentar una reclamación ante la autoridad de control:\n" +
            "En todo momento, cualquier afectado por la política de tratamiento de datos podrá presentar una reclamación ante la Agencia de Protección de Datos.\n\n" +
            "Ley aplicable:\n" +
            "La ley aplicable en caso de disputa o conflicto de interpretación de los términos que conforman este aviso legal, así como cualquier cuestión relacionada con los servicios del presente portal, será la ley española vigente." +
            "\nAl aceptar estos términos y condiciones el usuario afirma ser mayor de 14 años.";
    }
}
