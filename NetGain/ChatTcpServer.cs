using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NetGain.Models;
using Newtonsoft.Json;
using StackExchange.NetGain;

namespace NetGain
{
    internal class ChatTcpServer : TcpServer
    {
        public static Dictionary<Guid, User> Connections = new Dictionary<Guid, User>(); 

        public ChatTcpServer(int concurrentOperations) : base(concurrentOperations)
        {}
        public override void OnReceived(Connection connection, object value)
        {
            base.OnReceived(connection, value);

            Message<object> message = null;
            try
            {
               message = JsonConvert.DeserializeObject<Message<object>>(value.ToString());
            }
            catch (Exception e)
            {
                //TODO: Fill it with shit.
            }

            if (message == null)
                return;

            switch (message.Type)
            {
                case MessageType.MESSAGE:
                    ChatMessage chat = new ChatMessage()
                    {
                        Message = message.Data?.ToString(),
                        User = Connections.FirstOrDefault(pair => pair.Key.Equals((Guid) connection.UserToken)).Value?.Name
                    };

                    Broadcast(JsonConvert.SerializeObject(new Message<ChatMessage>()
                    {
                        Data = chat,
                        Type = MessageType.MESSAGE
                    }));
                    break;
                case MessageType.SEND_NAME:
                    Connections.FirstOrDefault(pair => pair.Key.Equals((Guid) connection.UserToken)).Value.Name =
                        message.Data.ToString();

                    Broadcast(JsonConvert.SerializeObject(new Message<List<User>>()
                    {
                        Data = ChatTcpServer.Connections.Where(pair => pair.Value.Connection.IsAlive).Select(pair => pair.Value).ToList(),
                        Type = MessageType.USER_UPDATE
                    }));

                    break;
            }
        }

        public override void OnAfterAuthenticate(Connection connection)
        {
            base.OnAfterAuthenticate(connection);

            connection.UserToken = Guid.NewGuid();
            ChatTcpServer.Connections.Add((Guid)connection.UserToken, new User()
            {
                Id = (Guid) connection.UserToken,
                Connection = connection
            });

            Message<object> message = new Message<object>()
            {
                Type = MessageType.SEND_NAME
            };

            connection.Send(Context, JsonConvert.SerializeObject(message));
        }

        //public override void OnClosing(Connection connection)
        //{
            
        //}
        

        public override void OnAuthenticate(Connection connection, StringDictionary claims)
        {
            base.OnAuthenticate(connection, claims);



        }
    }
}
