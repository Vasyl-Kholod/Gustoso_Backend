using Gustoso.Common.DTO.Communication;
using Gustoso.Common.Helpers;
using Gustoso.Common.IServices;
using Microsoft.Extensions.Configuration;
using SuperWebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Socket
{
    public partial class GustosoSocketServer: IGustosoSocketServer
    {
        private readonly WebSocketServer _server;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<NotificationContainer<object>> _notificationQueue;

        public GustosoSocketServer(IConfigurationRoot root, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _server = new WebSocketServer();

            var socketSection = root.GetSection("SocketServer");
            int port = Convert.ToInt32(socketSection["port"]);
            if (_server.Setup(port))
            {
                _server.Start();
            }

            _server.NewMessageReceived += OnNewMessageReceived;
            _server.SessionClosed += OnSessionClosed;

            var queueChecker = TimerConfigurator.GetConfiguredTimer(1, SendNotificationFromQueue);
            _notificationQueue = new ConcurrentQueue<NotificationContainer<object>>();
            queueChecker.Start();
        }
    }
}
