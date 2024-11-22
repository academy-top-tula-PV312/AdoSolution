using AdoParametersApp;
using Microsoft.Data.SqlClient;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoParametersApp
{
    static class Examples
    {
        public static async Task ParamForInsertExamples()
        {
            List<User> users = new List<User>()
            {
                new(){ Name = "Tommy", Age = 28 },
                new(){ Name = "Jimmy", Age = 42 },
                new(){ Name = "Benny", Age = 33 },
            };



            string connectionString = "Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                string queryString = @$"INSERT INTO users
                                    (name, age)
                                    VALUES
                                    (@name, @age)";
                command.CommandText = queryString;

                //SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
                //SqlParameter ageParam = new SqlParameter("@age", SqlDbType.Int);
                //command.Parameters.Add(nameParam);
                //command.Parameters.Add(ageParam);

                //foreach (var user in users)
                //{
                //    //string queryString = @$"INSERT INTO users
                //    //                            (name, age)
                //    //                            VALUES
                //    //                            ('{user.Name}', {user.Age})";


                //    nameParam.Value = user.Name;
                //    ageParam.Value = user.Age;

                //    await command.ExecuteNonQueryAsync();
                //}

                command.CommandText = "SELECT * FROM users WHERE name='Bobby'";

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write($"{reader.GetName(i)}\t");
                        Console.WriteLine($"\n{new string('-', 30)}");

                        while (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write($"{reader.GetValue(i)}\t");
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static async Task ParamDirectionExamples()
        {
            string connectionString = "Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                string queryString = @$"INSERT INTO users
                                    (name, age)
                                    VALUES
                                    (@name, @age);
                            SET @id = SCOPE_IDENTITY()";
                command.CommandText = queryString;

                SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
                SqlParameter ageParam = new SqlParameter("@age", SqlDbType.Int);
                command.Parameters.Add(nameParam);
                command.Parameters.Add(ageParam);

                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                idParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(idParam);

                nameParam.Value = "Poppy";
                ageParam.Value = 23;

                await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Last identity = {idParam.Value}");
            }
        }

        public static async Task StoredProceduresExample()
        {
            string connectionString = "Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = connection.CreateCommand();
                string queryString = "InsertUser";


                //@"CREATE PROCEDURE [dbo].[InsertUser]
                //     @name NVARCHAR(50),
                //     @age INT
                // AS
                // INSERT INTO users
                //         (name, age)
                //         VALUES
                //         (@name, @age)
                // SELECT SCOPE_IDENTITY()
                // GO;";

                //@"CREATE PROCEDURE[dbo].[GetUsers]
                //AS
                //SELECT* FROM users
                //GO;";

                command.CommandText = queryString;
                command.CommandType = CommandType.StoredProcedure;

                string name;
                int age;

                Console.Write("Input name: ");
                name = Console.ReadLine();

                Console.Write("Input age: ");
                age = Int32.Parse(Console.ReadLine());

                SqlParameter nameParam = new("@name", name);
                SqlParameter ageParam = new("@age", age);
                command.Parameters.Add(nameParam);
                command.Parameters.Add(ageParam);

                //await command.ExecuteNonQueryAsync();
                var id = await command.ExecuteScalarAsync();

                Console.WriteLine($"User add with id = {id}");

                ///////////

                SqlCommand commandGet = connection.CreateCommand();
                queryString = "GetUsers";
                commandGet.CommandText = queryString;
                commandGet.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await commandGet.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write($"{reader.GetName(i)}\t");
                        Console.WriteLine($"\n{new string('-', 30)}");

                        while (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                Console.Write($"{reader.GetValue(i)}\t");
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static async Task TransactionExamples()
        {
            string connectionString = "Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();

                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO users (name, age) VALUES ('Henry', 45)";
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "INSERT INTO users (name, age) VALUES ('Olly', 'Hello')";
                    await command.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                }
            }
        }

        public static async Task BinaryToDatabase()
        {
            string connectionString = "Data Source=3-0;Initial Catalog=air_flights_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = connection.CreateCommand();

                string fileName = @"D:\RPO\MaxEfimov\ariplanes\logos\logo-aeroflot-02.png";

                string queryString = @"UPDATE airlines SET logotype = @image WHERE id = @id";
                command.CommandText = queryString;

                SqlParameter idParam = new("@id", SqlDbType.Int);
                command.Parameters.Add(idParam);
                idParam.Value = 1;

                SqlParameter imageParam = new("@image", SqlDbType.Binary);
                command.Parameters.Add(imageParam);

                byte[] image;
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    image = new byte[stream.Length];
                    stream.Read(image, 0, image.Length);
                    imageParam.Value = image;
                }

                await command.ExecuteNonQueryAsync();

            }
        }
    }
}
