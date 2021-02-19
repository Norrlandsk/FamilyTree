using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FamilyTree
{
    internal class People
    {
        public static List<Person> listOfPeople = new List<Person>();

        #region CRUD

        public static void CreatePerson(Person person)
        {
            var firstNameParam = ("@firstName", person.FirstName);
            var lastNameParam = ("@lastName", person.LastName);
            var yearParam = ("@year", person.Year);
            var fatherIdParam = ("@fatherId", person.FatherId);
            var motherIdParam = ("@motherId", person.MotherId);

            var sql = @"INSERT INTO People VALUES (@firstName, @lastName, @year, @fatherId, @motherId)";
            long rowsAffected = SQLDatabase.ExecuteSQL(sql, firstNameParam, lastNameParam, yearParam, fatherIdParam, motherIdParam);
        }

        public static void ReadPerson(Person person)
        {
            if (person != null)
            {
                var firstNameParam = ("@firstName", person.FirstName);

                var sql = $"SELECT * FROM People WHERE firstName LIKE @firstName";
                DataTable dt = SQLDatabase.GetDataTable(sql, firstNameParam);
                SQLDatabase.ReadDataTable(dt);
                Menu.ContinueAndClear();

            }
            else
            {
                Console.WriteLine("No results!");
                Menu.ContinueAndClear();
            }
        }

        public static void UpdatePerson(Person person)
        {
            Console.WriteLine("Id || First name || Last name || Year of birth || Father's Id || Mother's Id");
            Console.WriteLine($"[1] {person.FirstName}\n[2] {person.LastName}\n[3] {person.Year}\n[4] {person.FatherId}\n[5] {person.MotherId}\n\n");
            Console.WriteLine("Which parameter would you like to update?");

            int choice = Menu.ConfirmCorrectInput(5);
            switch (choice)
            {
                case 1:
                    Console.Write("Change first name to:");
                    person.FirstName = Console.ReadLine();
                    var firstNameParam = ("@firstName", person.FirstName);
                    var sql = $"UPDATE People SET firstName=@firstName WHERE id={person.}";
                    break;
            }
        }

        public static void DeletePerson(Person person)
        {
        }

        #endregion CRUD

        #region Search

        public static Person Search(List<Person> person)
        {
            Console.WriteLine("Enter first name of person: ");
            var search = Console.ReadLine();
            return person.Find(person => person.FirstName == search);
        }

        public static Person RefineSearch(List<Person> person)
        {
            Console.WriteLine("Which one did you mean?\n");
            int i = 0;
            int choice = 0;
            foreach (var item in person)
            {
                for (i = 0; i < person.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {item.FirstName}, {item.LastName}, {item.Year} ");
                }
            }
            choice = Convert.ToInt32(Console.ReadLine());
            return person[choice - 1];
        }

        public static void ShowFamilyTree()
        {
            var sql = "SELECT * FROM People ORDER BY id";

            DataTable dt = SQLDatabase.GetDataTable(sql);
            SQLDatabase.ReadDataTable(dt);
        }

        #endregion Search

        #region Database and Table

        public static void CreateDatabase()
        {
            var sql = "CREATE DATABASE FamilyTree";
            SQLDatabase.ExecuteSQL(sql);
            SQLDatabase.DatabaseName = "FamilyTree";
        }

        public static void CreateTable()
        {
            var sql = @"CREATE TABLE People (
                        id int NOT NULL Identity (1,1),
                        firstName varchar(50),
                        lastName varchar(50),
                        year int,
                        fatherId int,
                        motherId int
                    ); ";
            SQLDatabase.ExecuteSQL(sql);
        }

        public static void PopulateDatabase()
        {
            //Första gen
            Person person1 = new Person("John", "Doe", 1984, 2, 3);

            //Andra gen
            Person person2 = new Person("John Sr", "Doe", 1963, 4, 5);
            Person person3 = new Person("Jane", "Doe", 1965, 6, 7);

            //Tredje gen
            Person person4 = new Person("Ludwig", "Grünwald", 1945, 0, 0);
            Person person5 = new Person("Jean-Marie", "LaMontagne", 1945, 0, 0);

            //Fjärde gen
            Person person6 = new Person("Sven", "Stenqvist", 1943, 0, 0);
            Person person7 = new Person("Amalia", "Jensen", 1942, 0, 0);

            listOfPeople.Add(person1);
            listOfPeople.Add(person2);
            listOfPeople.Add(person3);
            listOfPeople.Add(person4);
            listOfPeople.Add(person5);
            listOfPeople.Add(person6);
            listOfPeople.Add(person7);

            var sql =
         @$"INSERT INTO People VALUES ('{person1.FirstName}','{person1.LastName}', '{person1.Year}', '{person1.FatherId}', '{person1.MotherId}')
            INSERT INTO People VALUES ('{person2.FirstName}','{person2.LastName}','{person2.Year}','{person2.FatherId}', '{person2.MotherId}')

            INSERT INTO People VALUES ('{person3.FirstName}','{person3.LastName}','{person3.Year}','{person3.FatherId}', '{person3.MotherId}')

            INSERT INTO People VALUES ('{person4.FirstName}','{person4.LastName}','{person4.Year}','{person4.FatherId}', '{person4.MotherId}')
            INSERT INTO People VALUES ('{person5.FirstName}','{person5.LastName}','{person5.Year}','{person5.FatherId}', '{person5.MotherId}')

            INSERT INTO People VALUES ('{person6.FirstName}','{person6.LastName}','{person6.Year}','{person6.FatherId}', '{person6.MotherId}')
            INSERT INTO People VALUES ('{person7.FirstName}','{person7.LastName}','{person7.Year}','{person7.FatherId}', '{person7.MotherId}')
";
            SQLDatabase.ExecuteSQL(sql);
        }

        public static void Setup()
        {
            CreateDatabase();
            CreateTable();
            PopulateDatabase();
            Console.WriteLine("Database created, table populated!");
            Menu.ContinueAndClear();
        }

        public static void AltSetup()
        {
            CreateDatabase();
            CreateTable();
            Console.WriteLine("Database created, table empty!");
            Menu.ContinueAndClear();
        }

        #endregion Database and Table
    }
}