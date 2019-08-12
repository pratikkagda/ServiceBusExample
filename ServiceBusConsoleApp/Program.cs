using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ServiceBusConsoleApp
{
    class Program
    {
        static ITopicClient topicClient;
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            string ServiceBusConnectionString = "Endpoint=sb://pkservicebus.servicebus.windows.net/;SharedAccessKeyName=PK-Send;SharedAccessKey=jxq+zqeI8TlAT0CuwqYMmA0JuNFC4Hm9L9hlrO6NyNI=";
            string TopicName = "PK-Send";

            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            // Send messages.
            await SendUserMessage();

            Console.ReadKey();

            await topicClient.CloseAsync();
        }


        static async Task SendUserMessage()
        {
            List<User> users = GetDummyDataForUser();

            var serializeUser = JsonConvert.SerializeObject(users);

            string messageType = "userData";

            string messageId = Guid.NewGuid().ToString();

            var message = new ServiceBusMessage
            {
                Id = messageId,
                Type = messageType,
                Content = serializeUser
            };

            var serializeBody = JsonConvert.SerializeObject(message);

            // send data to bus

            var busMessage = new Message(Encoding.UTF8.GetBytes(serializeBody));
            busMessage.UserProperties.Add("Type", messageType);
            busMessage.MessageId = messageId;

            try
            {
                await topicClient.SendAsync(busMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            Console.WriteLine("message has been sent");

        }

        public class User
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class ServiceBusMessage
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
        }

        private static List<User> GetDummyDataForUser()
        {
            User user = new User();
            List<User> lstUsers = new List<User>();
            for (int i = 1; i < 3; i++)
            {
                user = new User();
                user.Id = i;
                user.Name = "CPVariyani" + i;

                lstUsers.Add(user);
            }

            return lstUsers;
        }
    }
}



#region Backup Code

/*
 
    static async Task SendMessage()
        {
            string connectionString = "Endpoint=sb://pkservicebus.servicebus.windows.net/;SharedAccessKeyName=PK-Send;SharedAccessKey=jxq+zqeI8TlAT0CuwqYMmA0JuNFC4Hm9L9hlrO6NyNI=;";
            string topicName = "PK-Send";

            var topicClient = new TopicClient(connectionString, topicName);

            //send message
            await SendUserMessage();
            Console.ReadKey();

            await topicClient.CloseAsync();
        }

        static async Task SendUserMessage()
        {
            List<User> users = GetUser();
            var serializeUser = JsonConvert.SerializeObject(users);
            string messageType = "userData";
            string messageId = Guid.NewGuid().ToString();

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage
            {
                Id = messageId,
                Type = messageType,
                Content =  serializeUser
            };

            var serializeBody = JsonConvert.SerializeObject(serviceBusMessage);

            //send data to the bus
            var busMessage = new Message(Encoding.UTF8.GetBytes(serializeBody));
            busMessage.UserProperties.Add("Type", messageType);
            busMessage.MessageId = messageId;

            await topicClient.SendAsync(busMessage);
            Console.WriteLine("Message has sent");
        }

        private static List<User> GetUser()
        {
            List<User> users = new List<User>()
            {
                new User{Id = 1, Name = "Pratik"},
                new User{ Id = 2, Name = "Archana"},
                new User {Id = 3, Name = "Hirva"}
            };

            return users;
        }

    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceBusMessage
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }

 */

#endregion