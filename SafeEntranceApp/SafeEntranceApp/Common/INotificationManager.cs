using System;
using System.Collections.Generic;
using System.Text;

namespace SafeEntranceApp.Common
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        /*
         * Contiene todas las acciones necesarias para inicializar el gestor de notificaciones
         */
        void Initialize();

        /*
         * Envía una notificación al dispositivo
         */
        void SendNotification(bool manualRefresh, string title, string message, DateTime? notifyTime = null);

        /*
         * Procesa las notificaciones entrantes
         */
        void ReceiveNotification(string title, string message);
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
