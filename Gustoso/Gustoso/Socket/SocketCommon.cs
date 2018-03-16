using Gustoso.Common.DTO.Communication;
using Newtonsoft.Json;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Socket
{
    public partial class GustosoSocketServer
    {
        private async Task SendNotificationFromQueue()
        {
            while (!_notificationQueue.IsEmpty)
            {
                _notificationQueue.TryDequeue(out var notification);

                if (notification.Receivers == null || notification.Receivers.Count == 0)
                {
                    return;
                }

                var message = JsonConvert.SerializeObject(notification.Notification);

                foreach (var receiver in notification.Receivers)
                {
                    var session = _server.GetSessionByID(receiver);
                    if (session != null)
                    {
                        session.Send(message);
                    }
                }
            }
        }

        private void NotRecognizedRequest(WebSocketSession session)
        {
            var response = new Response<string>()
            {
                Error = new Error(400, "Request was not recognized")
            };

            SendResponse(session, response);
        }

        private void SendResponse<T>(WebSocketSession session, Response<T> response)
        {
            if (response.Error != null)
            {
                session.Send(JsonConvert.SerializeObject(response.Error));
                return;
            }

            session.Send(JsonConvert.SerializeObject(response.Data));
        }
    }
}
