using System;
using System.Collections.Generic;
using System.Data;

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
            var idParam = ("@id", person.Id);

            var sql = $"SELECT * FROM People WHERE id=@id";
            DataTable dt = SQLDatabase.GetDataTable(sql, idParam);
            SQLDatabase.ReadDataTable(dt);
            Menu.ContinueAndClear();
        }

        public static void UpdatePerson(Person person)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("First name || Last name || Year of birth || Father's Id || Mother's Id\n");
                Console.WriteLine($"{person.FirstName} {person.LastName} {person.Year} {person.FatherId} {person.MotherId}\n");
                Console.WriteLine("What would you like to update?\n");
                Console.WriteLine("[1] First name");
                Console.WriteLine("[2] Last name");
                Console.WriteLine("[3] Year");
                Console.WriteLine("[4] Father's Id");
                Console.WriteLine("[5] Mother's Id");
                Console.WriteLine("[6] Go to Main menu");

                int choice = Menu.ConfirmCorrectInput(6);

                switch (choice)
                {
                    case 1:
                        person.FirstName = ValuePrompt();
                        break;

                    case 2:
                        person.LastName = ValuePrompt();
                        break;

                    case 3:
                        person.Year = ValuePromptInt();
                        break;

                    case 4:
                        person.FatherId = ValuePromptInt();
                        break;

                    case 5:
                        person.MotherId = ValuePromptInt();
                        break;

                    case 6:
                        isRunning = false;
                        break;
                }

                var idParam = ("@id", person.Id);
                var firstNameParam = ("@firstName", person.FirstName);
                var lastNameParam = ("@lastName", person.LastName);
                var yearParam = ("@year", person.Year);
                var fatherIdParam = ("@fatherId", person.FatherId);
                var motherIdParam = ("@motherId", person.MotherId);

                var sql = @"UPDATE People
                            SET firstName=@firstName, lastName=@lastName, year=@year, fatherId=@fatherId, motherId=@motherId
                            WHERE id=@id;";
                long rowsAffected = SQLDatabase.ExecuteSQL(sql, idParam, firstNameParam, lastNameParam, yearParam, fatherIdParam, motherIdParam);
                Console.WriteLine($"{rowsAffected} row(s) affected!");
                Menu.ContinueAndClear();
            }
        }

        private static string ValuePrompt()
        {
            string newValue;
            Console.Write("New value: ");
            newValue = Console.ReadLine();
            return newValue;
        }

        private static int ValuePromptInt()
        {
            int confirmedChoice;
            do
            {
                Console.Write("New value: ");
                string menuChoiceString = Console.ReadLine();

                bool successfulConversion = Int32.TryParse(menuChoiceString, out confirmedChoice);

                if (successfulConversion)
                {
                    break;
                }
                else if (menuChoiceString == "")
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid input!\n");
                }
            } while (true);
            return confirmedChoice;
        }

        public static void DeletePerson(Person person)
        {
            Console.WriteLine("Are you sure you wish to delete this member? [y/n]");
            var choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {
                var sql = $"DELETE FROM People WHERE id={person.Id};";
                long rowsAffected = SQLDatabase.ExecuteSQL(sql);
                Console.WriteLine($"{rowsAffected} row(s) affected!");
                Menu.ContinueAndClear();
            }
            else
            {
                Menu.ContinueAndClear();
            }
        }

        #endregion CRUD

        #region Search

        public static Person Search()
        {
            bool isRunning = true;
            Person person = new Person();
            while (isRunning)
            {
                Console.WriteLine("Please specify member to work with\n\n");
                Console.Write("Enter first name: ");
                var fName = Console.ReadLine();

                Console.Write("Enter last name: ");
                var lName = Console.ReadLine();

                var firstNameParam = ("@firstName", fName);
                var lastNameParam = ("@lastName", lName);

                var sql = $"SELECT * FROM People WHERE firstName LIKE @firstName AND lastName LIKE @lastName";
                DataTable dt = SQLDatabase.GetDataTable(sql, firstNameParam, lastNameParam);

                if (dt.Rows.Count == 1)
                {
                    FillPersonObject(dt, person);
                    break;
                }
                else if (dt.Rows.Count > 1)
                {
                    Console.WriteLine("Your query returned multiple results!\n\nPlease specify which member you meant\n\n");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        Console.WriteLine($"[{i + 1}]{row["firstName"]} {row["lastName"]} {row["year"]}");
                    }
                    int choice = Menu.ConfirmCorrectInput(dt.Rows.Count);
                    choice -= 1;

                    FillPersonObject(dt.Rows[choice], person);
                    break;
                }
                else
                {
                    Console.WriteLine("No results for that query! Try different spelling or add % as a wildcard");
                }
            }
            return person;
        }

        public static void Test(Person person)
        {
            Console.WriteLine($"{person.FirstName}{person.LastName}{person.Year}{person.FatherId}{person.MotherId}");
        }

        private static void FillPersonObject(DataTable dt, Person person)
        {
            person.Id = (int)dt.Rows[0]["id"];
            person.FirstName = (string)dt.Rows[0]["firstName"];
            person.LastName = (string)dt.Rows[0]["lastName"];
            person.Year = (int)dt.Rows[0]["year"];
            person.FatherId = (int)dt.Rows[0]["fatherId"];
            person.MotherId = (int)dt.Rows[0]["motherId"];
        }

        private static void FillPersonObject(DataRow row, Person person)
        {
            person.Id = (int)row["id"];
            person.FirstName = (string)row["firstName"];
            person.LastName = (string)row["lastName"];
            person.Year = (int)row["year"];
            person.FatherId = (int)row["fatherId"];
            person.MotherId = (int)row["motherId"];
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
            var sql =
         @$"INSERT INTO People VALUES ('Viktor', 'Salmberg', '1990', '4','5')
            INSERT INTO People VALUES ('David', 'Salmberg', '1992', '4', '5')
            INSERT INTO People VALUES ('Josefina', 'Salmberg', '1977', '4', '5')

            INSERT INTO People VALUES ('Ronny', 'Salmberg', '1945','0','0')
            INSERT INTO People VALUES ('Gunilla', 'Andersson', '1951','0','0')
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