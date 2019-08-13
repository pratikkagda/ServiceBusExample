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
        const string ServiceBusConnectionString = "Endpoint=sb://pkservicebus.servicebus.windows.net/;SharedAccessKeyName=PK-Send;SharedAccessKey=jxq+zqeI8TlAT0CuwqYMmA0JuNFC4Hm9L9hlrO6NyNI=";
        const string TopicName = "pkservicebustopic";
        //static ITopicClient topicClient;
        static void Main(string[] args)
        {
            SendMessage().GetAwaiter().GetResult();
        }

        static async Task SendMessage()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

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
                Content = serializeUser
            };

            var serializeBody = JsonConvert.SerializeObject(serviceBusMessage);

            //send data to the bus
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
}