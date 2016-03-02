using System;
using Newtonsoft.Json;
using StackExchange.NetGain;

namespace NetGain.Models
{
    class User
    {
        public User()
        {
            ConnectedSince = DateTime.UtcNow;
        }
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("connectedSince")]
        public DateTime ConnectedSince { get; set; } 

        [JsonIgnore]
        public Connection Connection { get; set; }

    }
}
