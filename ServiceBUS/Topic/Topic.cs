using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBUSTopic
{
    class Program
    {
        const string ServiceBusConnectionString = "Connection String from Access Keys or Shared Access Signature";
        const string topicName = "mytopic";
        const string subscriptionName = "mysubscription";

        static ITopicClient topicClient;
        static ISubscriptionClient subscriptionClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, topicName);
            Console.WriteLine("======================================================");
            Console.WriteLine("Service BUS Topic Operations");
            Console.Write("======================================================");
            await SendMessagesAsync(5);
            ReceiveMessages();
            Console.ReadKey();
            await subscriptionClient.CloseAsync();
        }

        static void ReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.Write($"\nReceived message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the queue
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.Write($"\nSending message: {messageBody}");

                    // Send the message to the queue
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
