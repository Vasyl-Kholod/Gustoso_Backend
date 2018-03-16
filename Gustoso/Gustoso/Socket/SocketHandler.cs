using System;
using Gustoso.Common.Sockets;
using SuperWebSocket;
using static Gustoso.Common.Helpers.Deserializer;

namespace Gustoso.Socket
{
    public partial class GustosoSocketServer
    {
        private void OnNewMessageReceived(WebSocketSession session, string rawMessage)
        {
            var baseMessage = Deserialize<BaseMessage>(rawMessage);
            if (baseMessage == null)
            {
                NotRecognizedRequest(session);
                return;
            }
            session.Send(rawMessage);
        }

        private async void OnSessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {

        }
    }
}
