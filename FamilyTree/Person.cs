using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace FamilyTree
{
    class Person
    {
        public Person()
        {

        }

        public Person( string firstName, string lastName, int year, int fatherId = 0, int motherId = 0, int id = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Year = year;
            FatherId = fatherId;
            MotherId = motherId;

        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public int FatherId { get; set; }
        public int MotherId { get; set; }

        public static void CreatePerson()
        {
            Person person = new Person();
            Console.Write("First name: ");
            person.FirstName = Console.ReadLine();
            Console.Write("Last name: ");
            person.LastName = Console.ReadLine();
            Console.Write("Year of birth: ");
            person.Year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Father's id: ");
            person.FatherId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Mother's id: ");
            person.MotherId = Convert.ToInt32(Console.ReadLine());

           
            People.CreatePerson(person);
        }
        


        public static void Read(Person person)
        {

            Console.WriteLine($"{person.FirstName} {person.LastName} {person.Year} {person.FatherId} {person.MotherId}");


        }


    }
}
