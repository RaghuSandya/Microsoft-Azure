using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    class Program
    {
        private static Random random = new Random();
        private static Employee employee;
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = "Connection String from Access Keys";
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("Azure Redis Cache Operations");
            Console.WriteLine("=========================================\n");
            IDatabase cache = lazyConnection.Value.GetDatabase();
            while (true)
            {
                Console.WriteLine("1. Insert Data\n2. Get Data\n3. Update Data\n4. Delete Data\n5. Exit\n");
                Console.Write("Enter your Option : ");
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        employee = new Employee(RandomString(random, 2)+ random.Next(1, 999), RandomString(random, 20), random.Next(999, 999999), random.Next(1, 99));
                        if(cache.StringSet("e01", JsonConvert.SerializeObject(employee)))
                        {
                            Console.WriteLine("Data inserted sucessfully.");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Data From Cache Service : " + cache.StringGet("e01"));
                        break;
                    case 3:
                        employee = new Employee(RandomString(random, 2) + random.Next(1, 999), RandomString(random, 20), random.Next(999, 999999), random.Next(1, 99));
                        if (cache.StringSet("e01", JsonConvert.SerializeObject(employee)))
                        {
                            Console.WriteLine("Data updated sucessfully.");
                        }
                        break;
                    case 4:
                        cache.KeyDelete("e01");
                        Console.WriteLine("Data deleted successfully. ");
                        break;
                    case 5:
                        System.Environment.Exit(1);
                        break;
                }
            }
        }
        public static string RandomString(Random random, int length)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
    
    class Employee
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public int Age { get; set; }

        public Employee(string EmployeeId, string Name, int Salary, int Age)
        {
            this.EmployeeId = EmployeeId;
            this.Name = Name;
            this.Salary = Salary;
            this.Age = Age;
        }
    }
}
