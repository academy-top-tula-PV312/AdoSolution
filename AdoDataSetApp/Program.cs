using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
string queryString = "SELECT * FROM users";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
    DataSet data = new DataSet();
    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
    
    DataSetFillAndPrint(adapter, data);
    Console.WriteLine();

    DataTable table = data.Tables["users"];

    //DataRow userRow = table.NewRow();
    //userRow["name"] = "Poppy";
    //userRow["age"] = 25;
    //table.Rows.Add(userRow);

    //table.Rows[0]["age"] = 20;

    var deleteRow = table.Rows[1];

    table.Rows[1].Delete();

    Console.WriteLine($"rows = {table.Rows.Count}");

    //adapter.Update(data);

    data.Clear();

    DataSetFillAndPrint(adapter, data);
    Console.WriteLine();
}


void DataSetFillAndPrint(SqlDataAdapter adapter, DataSet data)
{
    adapter.Fill(data);

    foreach (DataTable table in data.Tables)
    {
        foreach (DataColumn column in table.Columns)
        {
            Console.Write($"{column.ColumnName}\t");
        }
        Console.WriteLine($"\n{new string('-', 20)}");
        foreach (DataRow row in table.Rows)
        {
            object[] cells = row.ItemArray;
            foreach (var cell in cells)
            {
                Console.Write($"{cell}\t");
            }
            Console.WriteLine();
        }

    }

}