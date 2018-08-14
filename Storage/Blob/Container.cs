using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBlobContainer
{
    class Program
    {
        static CloudStorageAccount storageAccount;
        static CloudBlobClient blobClient;
        static CloudBlobContainer blobContainer;
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("Azure Storage Blob Container Operations");
            Console.WriteLine("=========================================");

            while (true)
            {
                string containerName = null;
                storageAccount = CloudStorageAccount.Parse("Shared Access Signature Primary or Secondary Connection String");
                blobClient = storageAccount.CreateCloudBlobClient();
                Console.WriteLine("\n1. Create a container\n2. View Containers\n3. Delete a container\n4. Exit\n");
                Console.Write("Enter your Option : ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        // Create a container if it doesn't exist.
                        Console.Write("Enter name of Container : ");
                        containerName = Console.ReadLine();
                        blobContainer = blobClient.GetContainerReference(containerName);
                        blobContainer.CreateIfNotExistsAsync();
                        blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                        Console.WriteLine("Container created successfully.");
                        break;
                    case 2:
                        // View all containers
                        IEnumerable<CloudBlobContainer> containers = blobClient.ListContainers();
                        foreach (CloudBlobContainer list in containers)
                        {
                            Console.WriteLine("Container Name : " + list.Name + "\tContainer Link: " + list.Uri);
                        }
                        break;
                    case 3:
                        // Delete a container
                        Console.Write("Enter name of Container : ");
                        containerName = Console.ReadLine();
                        blobContainer = blobClient.GetContainerReference(containerName);
                        blobContainer.DeleteIfExistsAsync();
                        Console.WriteLine("Container deleted successfully.");
                        break;
                    case 4:
                        System.Environment.Exit(1);
                        break;
                }
            }
        }
    }
}
