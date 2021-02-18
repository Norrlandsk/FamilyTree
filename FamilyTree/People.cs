using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace FamilyTree
{
    class People
    {

        #region CRUD
        public static void Create(Person person)
        {


            var firstNameParam = ("@firstName", person.FirstName);
            var lastNameParam = ("@lastName", person.LastName);
            var yearParam = ("@year", person.Year);
            var fatherIdParam = ("@fatherId", person.FatherId);
            var motherIdParam = ("@motherId", person.MotherId);

            var sql = @"INSERT INTO People VALUES (@firstName, @lastName, @year, @fatherId, @motherId)";
            long rowsAffected = SQLDatabase.ExecuteSQL(sql, firstNameParam, lastNameParam, yearParam, fatherIdParam, motherIdParam);


        }
        #endregion CRUD

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

        public static void AddInitialPeople()
        {
            Person person1 = new Person("John", "Doe", 1984, 2, 3);

            Person person2 = new Person("John Sr", "Doe", 1963, 4, 5);
            Person person3 = new Person("Jane", "Doe", 1965, 6, 7);

            Person person4 = new Person("Ludwig", "Grünwald", 1945, 0, 0);
            Person person5 = new Person("Jean-Marie", "LaMontagne", 1945, 0, 0);

            Person person6 = new Person("Sven", "Stenqvist", 1943, 0, 0);
            Person person7 = new Person("Amalia", "Jensen", 1942, 0, 0);

            var sql1 = $"INSERT INTO People VALUES ('{person1.FirstName}', '{person1.LastName}', '{person1.Year}', '{person1.FatherId}', '{person1.MotherId}')";

            var sql2 = $"INSERT INTO People VALUES ('{person2.FirstName}','{person2.LastName}','{person2.Year}','{person2.FatherId}', '{person2.MotherId}')";
            var sql3 = $"INSERT INTO People VALUES ('{person3.FirstName}','{person3.LastName}','{person3.Year}','{person3.FatherId}', '{person3.MotherId}')";

            var sql4 = $"INSERT INTO People VALUES ('{person4.FirstName}','{person4.LastName}','{person4.Year}','{person4.FatherId}', '{person4.MotherId}')";
            var sql5 = $"INSERT INTO People VALUES ('{person5.FirstName}','{person5.LastName}','{person5.Year}','{person5.FatherId}', '{person5.MotherId}')";

            var sql6 = $"INSERT INTO People VALUES ('{person6.FirstName}','{person6.LastName}','{person6.Year}','{person6.FatherId}', '{person6.MotherId}')";
            var sql7 = $"INSERT INTO People VALUES ('{person7.FirstName}','{person7.LastName}','{person7.Year}','{person7.FatherId}', '{person7.MotherId}')";

            SQLDatabase.ExecuteSQL(sql1);
            SQLDatabase.ExecuteSQL(sql2);
            SQLDatabase.ExecuteSQL(sql3);
            SQLDatabase.ExecuteSQL(sql4);
            SQLDatabase.ExecuteSQL(sql5);
            SQLDatabase.ExecuteSQL(sql6);
            SQLDatabase.ExecuteSQL(sql7);
        }

        public static void Setup()
        {
            CreateDatabase();
            CreateTable();
            AddInitialPeople();
            Person.CreatePerson();
        }
        #endregion Database and Table
    }



}
