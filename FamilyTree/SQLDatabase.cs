using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace FamilyTree
{
    public class SQLDatabase
    {
        
        private static string ConnectionString { get; set; } = @"Data source=.\SQLExpress; Integrated Security=true; Database={0}";
        public static string DatabaseName { get; set; }


        public static DataTable GetDataTable(string sql, params (string, object)[] parameters)
        {
            //Instansierar en ny tom tabell att fylla
            var dataTable = new DataTable();

            //Skapar en koppling till databasen med propertyn ConnectionString
            ConnectionString = string.Format(ConnectionString, DatabaseName);
            var connection = new SqlConnection(ConnectionString);

            //Aktiverar kopplingen till databasen
            connection.Open();

            //Skickar query till databasen (sqlString), och försäkrar oss om att det är rätt databas vi skickar till (connection)
            var command = new SqlCommand(sql, connection);

            //Lägger till parametrar att skicka till servern

            SetParameters(parameters, command);

            //Konverterar datan och fyller tabellen (dataTable)
            new SqlDataAdapter(command).Fill(dataTable);

            //Stänger kopplingen till databasen
            connection.Close();

            return dataTable;
        }
        private static void SetParameters((string, object)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }
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

        public static void ReadDataTable(DataTable dT)
        {
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
            Menu.ContinueAndClear();
        }
    }
}
