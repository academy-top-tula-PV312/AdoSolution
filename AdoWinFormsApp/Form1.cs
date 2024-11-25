using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ado_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            string queryString = "SELECT * FROM users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                DataSet data = new DataSet();
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                adapter.Fill(data);

                dataGridViewUsers.AutoGenerateColumns = true;
                dataGridViewUsers.DataSource = data.Tables[0];
                //dataGridViewUsers.DataMember = "users";
            }

                
        }
    }
}
