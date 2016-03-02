using Newtonsoft.Json;

namespace NetGain.Models
{
    class ChatMessage
    {
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
