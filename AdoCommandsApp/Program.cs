using AdoCommandsApp;
using Microsoft.Data.SqlClient;

string connectionString = "Data Source=3-0;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();

    string queryString = //"SELECT AVG(age) FROM users";

                        "SELECT * FROM users";
                        
                        //@"INSERT INTO users
                        //(name, age)
                        //VALUES
                        //('Bobby', 35),
                        //('Sammy', 28),
                        //('Lenny', 31)";
                        
                         //@"CREATE TABLE users 
                         //(
                         //   id INT IDENTITY PRIMARY KEY,
                         //   name NVARCHAR(50) NOT NULL,
                         //   age INT NULL
                         //)"; 
                        //"CREATE DATABASE ado_db";

    SqlCommand command1 = new SqlCommand();
    command1.CommandText = queryString;
    command1.Connection = connection;

    SqlCommand command2 = new SqlCommand(queryString);
    command2.Connection = connection;

    SqlCommand command3 = new SqlCommand(queryString, connection);

    SqlCommand command = connection.CreateCommand();
    command.CommandText = queryString;

    //int numbers = await command.ExecuteNonQueryAsync();
    //Console.WriteLine($"inserts {numbers} users");

    List<User> users = new List<User>();

    using (SqlDataReader reader = await command.ExecuteReaderAsync())
    {
        if (reader.HasRows)
        {
            //for (int i = 0; i < reader.FieldCount; i++)
            //    Console.Write($"{reader.GetName(i)}\t");
            //Console.WriteLine($"\n{new string('-', 20)}");

            //while (await reader.ReadAsync())
            //{
            //    //for (int i = 0; i < reader.FieldCount; i++)
            //    //    Console.Write($"{reader.GetValue(i)}\t");
            //    Console.Write($"{reader.GetInt32(0)}\t");
            //    Console.Write($"{reader.GetString(1)}\t");
            //    Console.Write($"{reader.GetInt32(2)}\t");
            //    Console.WriteLine();
            //}

            while (await reader.ReadAsync())
            {
                User user = new User() 
                { 
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                };
                users.Add(user);
            }
        }
    }

    //object ageAvg = await command.ExecuteScalarAsync();
    //Console.WriteLine($"Avg age of users: {Convert.ToDouble(ageAvg)}");

}