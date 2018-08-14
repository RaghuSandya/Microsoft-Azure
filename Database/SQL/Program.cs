using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDatabase
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "servername.database.windows.net";
                builder.UserID = "username";
                builder.Password = "password";
                builder.InitialCatalog = "database name";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                StringBuilder sb = new StringBuilder();

                Console.WriteLine("\nAzure Sql Database Operations");
                Console.WriteLine("=========================================\n");
                while (true)
                {
                    Console.WriteLine("1. Create a Table\n2. Insert a record\n3. Read a record\n4. Update a record\n5. Delete a record\n6. Delete a Table\n7. Exit\n");
                    Console.Write("Enter your Option : ");
                    int ID = 0;
                    switch (Convert.ToInt32(Console.ReadLine()))
                    {
                        case 1:
                            // Create a table if it doesn't exist.
                            sb.Append("IF NOT EXISTS(SELECT * FROM sysobjects WHERE name = 'CUSTOMERS' AND xtype = 'U') BEGIN CREATE TABLE CUSTOMERS(ID INT IDENTITY(1,1), NAME VARCHAR(20) NOT NULL, AGE INT NOT NULL, ADDRESS CHAR(25), SALARY DECIMAL(18, 2), PRIMARY KEY(ID)) END");
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                if(command.ExecuteNonQuery() == -1)
                                {
                                    Console.WriteLine("\nTable created successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("\nTable creation failed.");
                                }
                            }
                            break;
                        case 2:
                            // Insert Operation
                            sb.Append("INSERT INTO CUSTOMERS (NAME,AGE,ADDRESS,SALARY) VALUES (@NAME, @AGE, @ADDRESS, @SALARY);");
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                command.Parameters.AddWithValue("@NAME", RandomString(random, 20));
                                command.Parameters.AddWithValue("@AGE", random.Next(1, 99));
                                command.Parameters.AddWithValue("@ADDRESS", "Bangalore");
                                command.Parameters.AddWithValue("@SALARY", random.Next(2500, 190000));
                                if (command.ExecuteNonQuery() >= 0)
                                {
                                    Console.WriteLine("\nRecord Inserted Successfully");
                                }
                                else
                                {
                                    Console.WriteLine("\nRecord insertion failed.");
                                }
                            }
                            break;
                        case 3:
                            // Read Operation                            
                            sb.Append("SELECT ID,NAME,AGE,ADDRESS,SALARY FROM CUSTOMERS");
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("\nID : " + reader.GetInt32(0) + "\tName : " + reader.GetString(1)+ "\tAge : " + reader.GetInt32(2) + "\tAddress : " + reader.GetString(3) + "Salary : " + reader.GetDecimal(4));
                                    }
                                }
                            }
                            Console.WriteLine("\nRecord Read Successfully");
                            break;
                        case 4:
                            // Update Operation
                            Console.WriteLine("\nEnter ID to update : ");
                            ID = Convert.ToInt32(Console.ReadLine());                                
                            sb.Append("UPDATE CUSTOMERS SET NAME = @NAME,AGE = @AGE,ADDRESS = @ADDRESS,SALARY = @SALARY WHERE ID = "+ID);
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                command.Parameters.AddWithValue("@NAME", RandomString(random, 20));
                                command.Parameters.AddWithValue("@AGE", random.Next(1, 99));
                                command.Parameters.AddWithValue("@ADDRESS", "Bangalore");
                                command.Parameters.AddWithValue("@SALARY", random.Next(2500, 190000));
                                if (command.ExecuteNonQuery() >= 0)
                                {
                                    Console.WriteLine("\nRecord Updated Successfully");
                                }
                                else
                                {
                                    Console.WriteLine("\nRecord Update failed.");
                                }
                            }
                            break;
                        case 5:
                            // Record Deletion
                            Console.WriteLine("\nEnter ID to update : ");
                            ID = Convert.ToInt32(Console.ReadLine());
                            sb.Append("DELETE FROM CUSTOMERS WHERE ID = " + ID);
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                if (command.ExecuteNonQuery() >= 0)
                                {
                                    Console.WriteLine("\nRecord deleted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("\nRecord Delete failed.");
                                }
                            }                           
                            break;
                        case 6:
                            // Table Deletion
                            sb.Append("DROP TABLE CUSTOMERS");
                            using (SqlCommand command = new SqlCommand(sb.ToString(), connection))
                            {
                                if (command.ExecuteNonQuery() == -1)
                                {
                                    Console.WriteLine("\nTable deleted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("\nTable Delete failed.");
                                }
                            }                           
                            break;
                        case 7:
                            connection.Close();
                            System.Environment.Exit(1);
                            break;
                    }
                    sb.Clear();
                }
            } 
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
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
}
