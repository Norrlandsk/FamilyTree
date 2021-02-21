using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FamilyTree
{
    public class SQLDatabase
    {
        //Properties for connecting to Database
        public static string ConnectionString { get; set; } = @"Data source=.\SQLExpress; Integrated Security=true; database='{0}'";

        public static string DatabaseName { get; set; }

        //Sends a sql query to the database and returns a DataTable
        public static DataTable GetDataTable(string sql, params (string, object)[] parameters)
        {
            var dataTable = new DataTable();

            ConnectionString = string.Format(ConnectionString, DatabaseName);
            var connection = new SqlConnection(ConnectionString);

            connection.Open();

            var command = new SqlCommand(sql, connection);

            SetParameters(parameters, command);

            new SqlDataAdapter(command).Fill(dataTable);

            connection.Close();

            return dataTable;
        }

        //Sets the parameters used in the query
        private static void SetParameters((string, object)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }

        //Sends a sql query to the database and returns a long for rows affected
        public static long ExecuteSQL(string sqlString, params (string, object)[] parameters)
        {
            long rowsAffected = 0;
            try
            {
                var connString = string.Format(ConnectionString, DatabaseName);
                using (var cnn = new SqlConnection(connString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand(sqlString, cnn))
                    {
                        SetParameters(parameters, command);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return rowsAffected;
        }

        //Reads the Datatable
        public static void ReadDataTable(DataTable dT)
        {
            Console.Clear();
            Console.WriteLine("Id || First name || Last name || Year of birth || Father's Id || Mother's Id");
            string[] columnNames = dT.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

            foreach (DataRow row in dT.Rows)
            {
                for (int i = 0; i < columnNames.Length; i++)
                {
                    Console.Write($"{row[columnNames[i]]} ");
                }
                Console.WriteLine();
            }
        }
    }
}