using System.Data;
using Microsoft.Data.SqlClient;

DataSet data = new DataSet();
DataTable table = new DataTable("companies");

data.Tables.Add(table);

DataColumn idColumn = new DataColumn("id", Type.GetType("System.Int32"));
idColumn.Unique = true;
idColumn.AllowDBNull = false;
idColumn.AutoIncrement = true;
idColumn.AutoIncrementSeed = 1;
idColumn.AutoIncrementStep = 1;
table.Columns.Add(idColumn);

DataColumn titleColumn = new DataColumn("title", typeof(string));
titleColumn.Unique = true;
titleColumn.AllowDBNull = false;
table.Columns.Add(titleColumn);

DataColumn cityColumn = new DataColumn("city", typeof(string));
table.Columns.Add(cityColumn);

table.PrimaryKey = new DataColumn[] { table.Columns["id"] };

DataRow companyRow = table.NewRow();
companyRow.ItemArray = new object[] { null, "Yandex", "Moscow" };
table.Rows.Add(companyRow);


//string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
//using (SqlConnection connection = new SqlConnection(connectionString))
//{
//    SqlDataAdapter adapter = new SqlDataAdapter(connection);
//    adapter.Fill(data);
//    adapter.Update(data);
//}



