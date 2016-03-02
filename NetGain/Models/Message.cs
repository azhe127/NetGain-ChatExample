using Newtonsoft.Json;

namespace NetGain.Models
{
    internal class Message<T>
    {
        [JsonProperty("type")]
        public MessageType Type { get; set; }
        [JsonProperty("data")]
        public T Data{ get; set; }
    }
}
