using System.Collections.Generic;

namespace Gustoso.Common.DTO.Communication
{
    public class NotificationContainer<T> where T : class
    {
        public HashSet<string> Receivers { get; set; }

        public T Notification { get; set; }

        public NotificationContainer(HashSet<string> receivers, T notification)
        {
            Receivers = receivers;
            Notification = notification;
        }
    }
}
