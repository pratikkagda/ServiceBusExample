using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppServiceBus
{
    class Program
    {
        static ITopicClient topicClient;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();

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
            var busMessage = new ServiceBusMessage();
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
