using Newtonsoft.Json;

namespace Gustoso.Common.Sockets
{
    public class BaseMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; }

        public BaseMessage()
        {
            Type = GetType().Name;
        }
    }
}
