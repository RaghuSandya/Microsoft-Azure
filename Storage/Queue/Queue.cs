using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageQueue
{
    class Program
    {
        static CloudStorageAccount cloudStorageAccount;
        static CloudQueueClient cloudQueueClient;
        static CloudQueue cloudQueue;
        static CloudQueueMessage cloudQueueMessage;
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("Azure Storage Queue Operations");
            Console.WriteLine("=========================================");

            cloudStorageAccount = CloudStorageAccount.Parse("Shared Access Signature Primary or Secondary Connection String");
            cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();

            while (true)
            {
                string queueName = null;
                string queueMessage = null;
                Console.WriteLine("\n1. Create a Queue\n2. View queues\n3. Insert a message\n4. Peek a message\n5. Update a message\n6. Dequeue a message\n7. Delete a Queue\n8. Exit\n");
                Console.Write("Enter your Option : ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        // Create a queue if it doesn't exist.
                        Console.Write("Enter name of Queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        cloudQueue.CreateIfNotExists();
                        Console.WriteLine("Queue created successfully.");
                        break;
                    case 2:
                        // View all queues in the storage account
                        foreach (CloudQueue cloudQueues in cloudQueueClient.ListQueues())
                        {
                            Console.WriteLine("Name of the Queue : " + cloudQueues.Name);
                        }
                        break;
                    case 3:
                        // Insert a message in the queue
                        Console.Write("Enter name of Queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        Console.Write("Enter message : ");
                        queueMessage = Console.ReadLine();
                        cloudQueueMessage = new CloudQueueMessage(queueMessage);
                        cloudQueue.AddMessage(cloudQueueMessage);
                        Console.WriteLine("Queue message inserted successfully.");
                        break;
                    case 4:
                        // Peek a message in the queue
                        Console.Write("Enter name of Queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        cloudQueueMessage = cloudQueue.PeekMessage();
                        Console.WriteLine("Queue message : "+ cloudQueueMessage.AsString);
                        break;
                    case 5:
                        // Update a message in the queue
                        Console.Write("Enter name of Queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        cloudQueueMessage = cloudQueue.GetMessage();
                        cloudQueueMessage.SetMessageContent("Updated message in queue");
                        cloudQueue.UpdateMessage(cloudQueueMessage, TimeSpan.FromSeconds(1.0), MessageUpdateFields.Content | MessageUpdateFields.Visibility);
                        Console.WriteLine("Updated message in the Queue : " + cloudQueueMessage.AsString);
                        break;
                    case 6:
                        // Dequeue a message in the queue
                        Console.Write("Enter name of Queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        cloudQueueMessage = cloudQueue.GetMessage();
                        cloudQueue.DeleteMessage(cloudQueueMessage);
                        Console.WriteLine("Dequeue operation is successfull");
                        break;
                    case 7:
                        // Delete a queue in the storage account
                        Console.Write("Enter name of queue : ");
                        queueName = Console.ReadLine();
                        cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                        cloudQueue.DeleteIfExists();
                        Console.WriteLine("Deleting queue operation is successfull");
                        break;
                    case 8:
                        System.Environment.Exit(1);
                        break;
                }               
            }
        }
    }
}
