using System;
using System.Data;
using System.Data.SqlClient;

namespace FamilyTree
{
    internal class People
    {
        #region CRUD

        //Parameterize properties of Person object and sends it to Database
        public static void CreatePerson(Person person)
        {
            var firstNameParam = ("@firstName", person.FirstName);
            var lastNameParam = ("@lastName", person.LastName);
            var yearParam = ("@year", person.Year);
            var fatherIdParam = ("@fatherId", person.FatherId);
            var motherIdParam = ("@motherId", person.MotherId);

            var sql = @"INSERT INTO People VALUES (@firstName, @lastName, @year, @fatherId, @motherId)";
            long rowsAffected = SQLDatabase.ExecuteSQL(sql, firstNameParam, lastNameParam, yearParam, fatherIdParam, motherIdParam);
            Menu.ContinueAndClear();
        }

        //Prompts user if want to delete person, if yes, deletes person from Database
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

        //Reads all info about chosen person
        public static void ReadPerson(Person person)
        {
            var idParam = ("@id", person.Id);

            var sql = $"SELECT * FROM People WHERE id=@id";
            DataTable dt = SQLDatabase.GetDataTable(sql, idParam);
            SQLDatabase.ReadDataTable(dt);
            Menu.ContinueAndClear();
        }

        //Lets user choose what property to update
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
                        Menu.ContinueAndClear();
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

        //Prompts user for new value to property in UpdatePerson()
        private static string ValuePrompt()
        {
            string newValue;
            Console.Write("New value: ");
            newValue = Console.ReadLine();
            return newValue;
        }

        //Prompts user for new value and confirms it is an int for property in UpdatePerson()
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

        //Prompts user for year in SearchByYear(), confirms it is an int
        private static int YearPrompt()
        {
            int confirmedChoice;
            do
            {
                Console.Write("Enter year: ");
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

        #endregion CRUD

        #region Search

        //Retrieves children based on Person object's Id
        public static void TraceChildren(Person person)
        {
            var sql = $"SELECT * FROM People WHERE fatherId={person.Id} OR motherId={person.Id}";
            var childList = SQLDatabase.GetDataTable(sql);
            Console.WriteLine($"{person.FirstName} {person.LastName}'s child(ren)");
            SQLDatabase.ReadDataTable(childList);
            Menu.ContinueAndClear();
        }

        private static int IfFatherRowNull(object indata)
        {
            int id = (int)indata;
            if ((int)indata == 0)
            {
                return id = 999;
            }
            else
            {
                return id;
            }
        }

        private static int IfMotherRowNull(object indata)
        {
            int id = (int)indata;
            if ((int)indata == 0)
            {
                return id = 999;
            }
            else
            {
                return id;
            }
        }

        //Retrieves parents and grandparents based on Person object's Id |||||||| CRASHES IF DATATABLE RETURNS EMPTY
        public static void TraceGrandparents(Person person)
        {
            var father = IfFatherRowNull(person.FatherId);
            var mother = IfMotherRowNull(person.MotherId);

            var sqlFather = $"SELECT * FROM People WHERE id={father}";
            var sqlMother = $"SELECT * FROM People WHERE id={mother}";
            DataTable fatherData = SQLDatabase.GetDataTable(sqlFather);
            DataTable motherData = SQLDatabase.GetDataTable(sqlMother);
            var PaternalfatherNext = IfFatherRowNull(fatherData.Rows[0][4]);
            var PaternalmotherNext = IfMotherRowNull(fatherData.Rows[0][5]);

            //IfFatherRowNull(PaternalfatherNext);
            //IfMotherRowNull(PaternalmotherNext);

            var sqlPaternalGrandfather = $"SELECT * FROM People WHERE id={PaternalfatherNext}";
            var sqlPaternalGrandmother = $"SELECT * FROM People WHERE id={PaternalmotherNext}";
            DataTable paternalGrandfatherData = SQLDatabase.GetDataTable(sqlPaternalGrandfather);
            DataTable paternalGrandmotherData = SQLDatabase.GetDataTable(sqlPaternalGrandmother);
            var maternalFatherNext = IfFatherRowNull(motherData.Rows[0][4]);
            var maternalMotherNext = IfMotherRowNull(motherData.Rows[0][5]);

            //IfFatherRowNull(maternalFatherNext);
            //IfMotherRowNull(maternalMotherNext);
            var sqlMaternalGrandfather = $"SELECT * FROM People WHERE id={maternalFatherNext}";
            var sqlMaternalGrandmother = $"SELECT * FROM People WHERE id={maternalMotherNext}";
            DataTable maternalGrandfatherData = SQLDatabase.GetDataTable(sqlMaternalGrandfather);
            DataTable maternalGrandmotherData = SQLDatabase.GetDataTable(sqlMaternalGrandmother);

            Console.WriteLine($"Child: {person.FirstName} {person.LastName}");
            foreach (DataRow row in fatherData.Rows)
            {
                Console.WriteLine($"Father: {row["firstName"]} {row["lastName"]}");
            }
            foreach (DataRow row in motherData.Rows)
            {
                Console.WriteLine($"Mother: {row["firstName"]} {row["lastName"]}");
            }

            foreach (DataRow row in paternalGrandfatherData.Rows)
            {
                Console.WriteLine($"Paternal grandfather: {row["firstName"]} {row["lastName"]}");
            }
            foreach (DataRow row in paternalGrandmotherData.Rows)
            {
                Console.WriteLine($"Paternal grandmother: {row["firstName"]} {row["lastName"]}");
            }
            foreach (DataRow row in maternalGrandfatherData.Rows)
            {
                Console.WriteLine($"Maternal grandfather: {row["firstName"]} {row["lastName"]}");
            }
            foreach (DataRow row in maternalGrandmotherData.Rows)
            {
                Console.WriteLine($"Maternal grandmother: {row["firstName"]} {row["lastName"]}");
            }
            Menu.ContinueAndClear();
        }

        //Prompts for a letter, retrieves all instances of matches by first name
        public static void SearchByLetter()
        {
            Console.Write("Enter first letter of name:");
            var choice = Console.ReadLine();
            choice = choice + "%";

            var letterParam = ("@letter", choice);

            var sql = "SELECT * FROM People WHERE firstname LIKE @letter";

            DataTable dt = SQLDatabase.GetDataTable(sql, letterParam);
            if (dt.Rows.Count > 0)
            {
                SQLDatabase.ReadDataTable(dt);
            }
            else
            {
                Console.WriteLine("No search results!");
            }
            Menu.ContinueAndClear();
        }

        //Prompts for a year, retrieves all matches
        public static void SearchByYear()
        {
            int choice = YearPrompt();

            var sql = $"SELECT * FROM People WHERE year LIKE '{choice}'";
            DataTable dt = SQLDatabase.GetDataTable(sql);
            SQLDatabase.ReadDataTable(dt);
            Menu.ContinueAndClear();
        }

        //Prompts user for name to search for in database, if multiple matches, prompts user for correct match, then sends DataTable and Person object to
        //FillPersonObject()
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
                    Console.WriteLine("No results for that query! Try different spelling or add % as a wildcard. Try again? [y/n]");
                    var choice = Console.ReadLine().ToLower();
                    if (choice=="n")
                    {
                        isRunning = false;
                    }
                    
                }
            }
            Menu.ContinueAndClear();
            return person;
        }

        //Shows all members of database
        public static void ShowFamilyTree()
        {
            var sql = "SELECT * FROM People ORDER BY id";

            DataTable dt = SQLDatabase.GetDataTable(sql);
            SQLDatabase.ReadDataTable(dt);
            Menu.ContinueAndClear();
        }

        //Fills Person object with properties from Search(), for use when only one row in DataTable
        private static void FillPersonObject(DataTable dt, Person person)
        {
            person.Id = (int)dt.Rows[0]["id"];
            person.FirstName = (string)dt.Rows[0]["firstName"];
            person.LastName = (string)dt.Rows[0]["lastName"];
            person.Year = (int)dt.Rows[0]["year"];
            person.FatherId = (int)dt.Rows[0]["fatherId"];
            person.MotherId = (int)dt.Rows[0]["motherId"];
        }

        //Fills Person object with properties from Search(), for use when DataTable returns multiple rows
        private static void FillPersonObject(DataRow row, Person person)
        {
            person.Id = (int)row["id"];
            person.FirstName = (string)row["firstName"];
            person.LastName = (string)row["lastName"];
            person.Year = (int)row["year"];
            person.FatherId = (int)row["fatherId"];
            person.MotherId = (int)row["motherId"];
        }

        #endregion Search

        #region Database and Table

        //Creates Database and Table and gives user empty table to work with
        public static void AltSetup()
        {
            CreateDatabase();
            CreateTable();
            Menu.ContinueAndClear();
        }

        //Creates Database and Table and populates table with members
        public static void Setup()
        {
            CreateDatabase();
            CreateTable();
            PopulateDatabase();

            Menu.ContinueAndClear();
        }

        //Checks if database exist before trying to create a new database, to be used in CreateDatabase()
        internal static bool CheckDatabaseExists(string connectionstring, string databaseName)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        //Creates a new database named "FamilyTree"
        public static void CreateDatabase()
        {
            var connstring = @"Data source=.\SQLExpress; Integrated Security=true;";
            if (CheckDatabaseExists(connstring, "FamilyTree") == false)
            {
                var sql = "CREATE DATABASE FamilyTree";
                SQLDatabase.ExecuteSQL(sql);
                Console.WriteLine("Database created!");
            }
            else
            {
                Console.WriteLine("Database already exists, using existing database");
            }
            SQLDatabase.DatabaseName = "FamilyTree";
        }

        //Checks if database already contains a Table named "People", returns a bool for use in CreateTable()
        private static bool DoesDatatableExist()
        {
            bool datatableExist = false;
            SQLDatabase.ConnectionString = @"Data source=.\SQLExpress; Integrated Security=true; database='FamilyTree'";
            string sqlDatatableList = "SELECT * FROM information_schema.tables WHERE TABLE_NAME='People';";
            DataTable datatableList = SQLDatabase.GetDataTable(sqlDatatableList);

            if (datatableList.Rows.Count > 0)
            {
                datatableExist = true;
            }

            return datatableExist;
        }

        //Creates table
        public static void CreateTable()
        {
            bool DoesDatatableexist = DoesDatatableExist();
            if (!DoesDatatableexist)
            {
                var sql = @"
                        CREATE TABLE People (
                        id int NOT NULL Identity (1,1),
                        firstName varchar(50),
                        lastName varchar(50),
                        year int,
                        fatherId int,
                        motherId int
                    ); ";

                SQLDatabase.ConnectionString = @"Data source=.\SQLExpress; Integrated Security=true; database='FamilyTree'";
                SQLDatabase.ExecuteSQL(sql);
                Console.WriteLine("Datatable created!");
                
            }
            else
            {
                Console.WriteLine("Datatable already exists, using existing datatable");
            }
        }

        //Inserts data into table
        public static void PopulateDatabase()
        {
            var sql =

         @$"INSERT INTO People VALUES ('Viktor', 'Salmberg', '1990', '4','5')
            INSERT INTO People VALUES ('David', 'Salmberg', '1992', '4', '5')
            INSERT INTO People VALUES ('Josefina', 'Salmberg', '1977', '4', '5')

            INSERT INTO People VALUES ('Ronny', 'Salmberg', '1945','8','9')
            INSERT INTO People VALUES ('Gunilla', 'Andersson', '1951','10','11')
            INSERT INTO People VALUES ('Henry', 'Salmberg', '1939','8','9')
            INSERT INTO People VALUES ('John', 'Travolta', '1956','10','11')

            INSERT INTO People VALUES ('Folke', 'Persson', '1909','13','14')
            INSERT INTO People VALUES ('Elsa', 'Salmberg', '1911','15','16')

            INSERT INTO People VALUES ('Arthur', 'Andersson', '1915','17','18')
            INSERT INTO People VALUES ('Maja', 'Andersson', '1920','19','20')
            INSERT INTO People VALUES ('Godzilla', 'Hårddisksson', '1910','19','20')

            INSERT INTO People VALUES ('Arne', 'Grantén', '1880','0','0')
            INSERT INTO People VALUES ('Lidia', 'Kovacs', '1879','0','0')
            INSERT INTO People VALUES ('Reuben', 'Strongman', '1895','0','0')
            INSERT INTO People VALUES ('Julie', 'LaMontagne', '1889','0','0')

            INSERT INTO People VALUES ('Gaspar', 'Gasparov', '1880','0','0')
            INSERT INTO People VALUES ('Dierdre', 'Tokarova', '1878','0','0')

            INSERT INTO People VALUES ('Grant', 'Prescott', '1886','0','0')
            INSERT INTO People VALUES ('Lina', 'Hårddisksson', '1889','0','0')

";
            SQLDatabase.ExecuteSQL(sql);
        }

        #endregion Database and Table
    }
}