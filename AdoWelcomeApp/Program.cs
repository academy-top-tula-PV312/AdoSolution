using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

string connectionString = "Data Source=3-0;Initial Catalog=air_flights_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Pooling=False";
string connectionString2 = "Data Source=3-0;Initial Catalog=example;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

using (DbConnection connection = new SqlConnection(connectionString))
{
    try
    {
        //connection.Open();
        await connection.OpenAsync();
        Console.WriteLine("Connection to server!");
        Console.WriteLine((connection as SqlConnection).ClientConnectionId);

        //Console.WriteLine(connection.DataSource);
        //Console.WriteLine(connection.ServerVersion);
        //Console.WriteLine(connection.ConnectionString);
        //Console.WriteLine(connection.Database);
        //Console.WriteLine(connection.State);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

using (DbConnection connection = new SqlConnection(connectionString2))
{
    await connection.OpenAsync();
    Console.WriteLine("Connection to server!");
    Console.WriteLine((connection as SqlConnection).ClientConnectionId);
}

using (DbConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();
    Console.WriteLine("Connection to server!");
    Console.WriteLine((connection as SqlConnection).ClientConnectionId);
}



    