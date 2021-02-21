using System;

namespace FamilyTree
{
    internal class Person
    {
        public Person()
        {
        }

        public Person(string firstName, string lastName, int year, int fatherId = 0, int motherId = 0, int id = 0)
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

        //Creates an object of class Person and sets properties, then sends object to CRUD method CreatePerson()
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
    }
}