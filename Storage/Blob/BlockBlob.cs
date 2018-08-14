using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockBlob
{
    class Program
    {
        static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static string localFileName = "BlobFile_" + Guid.NewGuid().ToString() + ".txt";
        static string sourceFile = Path.Combine(localPath, localFileName);

        static CloudStorageAccount storageAccount;
        static CloudBlobClient blobClient;
        static CloudBlobContainer blobContainer;
        static CloudBlockBlob cloudBlockBlob;

        static void Main(string[] args)
        {
            string containerName = null;

            Console.WriteLine("=========================================");
            Console.WriteLine("Azure Storage Block Blob Operations");
            Console.WriteLine("=========================================");

            storageAccount = CloudStorageAccount.Parse("Connection String from Access Keys or Shared Access Signature");
            blobClient = storageAccount.CreateCloudBlobClient();

            Console.Write("Enter name of the container : ");
            containerName = Console.ReadLine();

            blobContainer = blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExistsAsync();
            blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            cloudBlockBlob = blobContainer.GetBlockBlobReference(localFileName);

            while (true)
            {
                Console.WriteLine("\n1. Upload a block blob\n2. View block blobs\n3. Download a block blob\n4. Delete a block blob\n5. Exit\n");
                Console.Write("Enter your Option : ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        // Create a Block Blob in the container
                        File.WriteAllText(sourceFile, "Hello, World For Block Blob File!");
                        cloudBlockBlob.UploadFromFileAsync(sourceFile);
                        Console.WriteLine("Block Blob Uploaded Successfully");
                        break;
                    case 2:
                        // View all Block Blobs in the container
                        foreach (IListBlobItem blob in blobContainer.ListBlobs(null, false))
                        {
                            Console.WriteLine("Primary Connection String : "+blob.StorageUri.PrimaryUri);
                            Console.WriteLine("Secondory Connection String : " + blob.StorageUri.SecondaryUri);
                        }
                        break;
                    case 3:
                        // Download a Block Blob in the container
                        cloudBlockBlob.DownloadToFileAsync(sourceFile, FileMode.Create);
                        Console.WriteLine("Block Blob Downloaded Successfully at : "+sourceFile);
                        break;
                    case 4:
                        // Delete all Block Blobs in the container
                        foreach (CloudBlockBlob blob in blobContainer.ListBlobs(null, false))
                        {
                            blob.DeleteIfExists();
                        }
                        Console.WriteLine("Deleting blob files completed");
                        break;
                    case 5:
                        System.Environment.Exit(1);
                        break;
                }
            }
        }
    }
}
