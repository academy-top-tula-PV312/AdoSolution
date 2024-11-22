using AdoParametersApp;
using Microsoft.Data.SqlClient;
using System.Data;

//await Examples.ParamForInsertExamples();

string connectionString = "Data Source=3-0;Initial Catalog=air_flights_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    await connection.OpenAsync();
    SqlCommand command = connection.CreateCommand();

    string queryString = "SELECT title, logotype FROM airlines WHERE id = 1";
    command.CommandText = queryString;

    using(SqlDataReader reader = await command.ExecuteReaderAsync())
    {
        if(reader.HasRows)
        {
            while(await reader.ReadAsync())
            {
                string fileName = reader.GetString(0);
                byte[] image = (byte[])reader.GetValue(1);

                using (FileStream stream = new FileStream(fileName + ".png", FileMode.Create))
                {
                    stream.Write(image);
                }
            }
        }
    }
}


