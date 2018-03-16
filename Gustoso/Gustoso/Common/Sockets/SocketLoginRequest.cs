using Newtonsoft.Json;

namespace Gustoso.Common.Sockets
{
    public class SocketLoginRequest : BaseMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }
    }
}
