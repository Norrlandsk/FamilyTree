using ConsoleTables;
using System;
using System.Data.SqlClient;

namespace FamilyTree
{
    class Program
    {
        static void Main(string[] args)
        {



            Menu.MainMenu();
            
        }

        private static void Print()
        {
            var table = new ConsoleTable("First Name", "Last Name", "Year of Birth");
            table.AddRow("Viktor", "Salmberg", 1990)
                 .AddRow("David", "Salmberg", 1992)
                 .AddRow("John", "Travolta", 1957);

            Console.WriteLine(table);
        }
    }
}
