using Microsoft.Azure;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTable
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("connectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("users");

            while(true)
            {
                Console.WriteLine("1. Create a Table\n2. Insert a record\n3. Read a record\n4. Update a record\n5. Delete a record\n6. Delete a Table\n7. Exit\n");
                Console.Write("Enter your Option : ");
                switch(Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        // Create a table if it doesn't exist.
                        table.CreateIfNotExists();
                        Console.WriteLine("Table created successfully.");
                        break;
                    case 2:
                        // Insert Operation
                        TableBatchOperation batchOperation = new TableBatchOperation();
                        Users users = new Users("abc", "Abc");
                        users.PhoneNumber = "9999999999";
                        users.Email = "abc.def@domain.com";
                        TableOperation insertOperation = TableOperation.Insert(users);
                        table.Execute(insertOperation);
                        Console.WriteLine("Record Inserted Successfully");
                        break;
                    case 3:
                        // Read Operation
                        TableQuery<Users> query = new TableQuery<Users>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "username"));
                        foreach (Users user in table.ExecuteQuery(query))
                        {
                            Console.WriteLine("User Name : {0}\tName : {1}\tEmail : {2}\tPhone Number : {3}\n", user.PartitionKey, user.RowKey, user.Email, user.PhoneNumber);
                        }
                        break;
                    case 4:
                        // Update Operation
                        TableOperation retrieveOperation = TableOperation.Retrieve<Users>("username", "name");
                        TableResult retrievedResult = table.Execute(retrieveOperation);
                        Users updateUser = (Users)retrievedResult.Result;
                        if (updateUser != null)
                        {
                            updateUser.PhoneNumber = "new phone number";
                            TableOperation updateOperation = TableOperation.Replace(updateUser);
                            table.Execute(updateOperation);
                            Console.WriteLine("User Name : {0}\tName : {1}\tEmail : {2}\tPhone Number : {3}\n", updateUser.PartitionKey, updateUser.RowKey, updateUser.Email, updateUser.PhoneNumber);
                            Console.WriteLine("User info updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("User info could not be retrieved to update.");
                        }
                        break;
                    case 5:
                        // Record Deletion
                        TableOperation deleteOperation = TableOperation.Retrieve<Users>("username", "name");
                        TableResult result = table.Execute(deleteOperation);
                        Users deleteUser = (Users)result.Result;
                        if (deleteUser != null)
                        {
                            deleteOperation = TableOperation.Delete(deleteUser);
                            table.Execute(deleteOperation);
                            Console.WriteLine("User info deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Could not retrieve the user info.");
                        }
                        break;
                    case 6:
                        // Table Deletion
                        table.DeleteIfExists();
                        Console.WriteLine("Table deleted successfully.");
                        break;
                    case 7:
                        System.Environment.Exit(1);
                        break;
                }
            }
        }
    }
    
    class Users : TableEntity
    {
        public Users(string UserName, string Name)
        {
            this.PartitionKey = UserName;
            this.RowKey = Name;
        }
        public Users() { }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
