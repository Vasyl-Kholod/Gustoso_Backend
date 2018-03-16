using Newtonsoft.Json;
using System;

namespace Gustoso.Common.Helpers
{
    public static class Deserializer
    {
        public static T Deserialize<T>(string rawMessage)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<T>(rawMessage);
                return json;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
