using System;

namespace FamilyTree
{
    internal class Menu
    {
        private static bool isRunning = true;

        #region Menus

        //Startup menu, will only be showed once during startup
        public static void MainMenu()
        {
            while (isRunning)
            {
                Console.WriteLine("Welcome to the Family Tree Database!\n\nWhat would you like to do?");
                Console.WriteLine("[1] Create a database and populate the Family Tree");
                Console.WriteLine("[2] Create a database, but let me populate the Family Tree myself");
                Console.WriteLine("[3] Exit the program");

                var menyChoice = ConfirmCorrectInput(3);

                switch (menyChoice)
                {
                    case 1:
                        People.Setup();
                        CRUDAndSearchMenu();
                        break;

                    case 2:
                        People.AltSetup();
                        CRUDAndSearchMenu();
                        break;

                    case 3:
                        Console.WriteLine("Exiting...");
                        isRunning = false;
                        break;
                }
            }
        }

        //Main menu for navigating the program's functions. Switch will send Person object to CRUD method
        public static void CRUDAndSearchMenu()
        {
            while (isRunning)
            {
                Console.WriteLine("CRUD\n");
                Console.WriteLine("[1] Create member");
                Console.WriteLine("[2] Read member");
                Console.WriteLine("[3] Update member");
                Console.WriteLine("[4] Delete member\n\n");

                Console.WriteLine("Searches\n");
                Console.WriteLine("[5] Show Family Tree");
                Console.WriteLine("[6] Show all members beginning with a certain letter");
                Console.WriteLine("[7] Show all members born a certain year");
                Console.WriteLine("[8] Show a member's parents and grandparents");
                Console.WriteLine("[9] Show a members children\n\n");

                Console.WriteLine("[10] Exit program");

                var menuChoice = ConfirmCorrectInput(10);

                switch (menuChoice)
                {
                    case 1:
                        Person.CreatePerson();
                        break;

                    case 2:
                        People.ReadPerson(People.Search());

                        break;

                    case 3:
                        People.UpdatePerson(People.Search());
                        break;

                    case 4:
                        People.DeletePerson(People.Search());
                        break;

                    case 5:
                        People.ShowFamilyTree();
                        break;

                    case 6:
                        People.SearchByLetter();
                        break;

                    case 7:
                        People.SearchByYear();
                        break;

                    case 8:
                        People.TraceGrandparents(People.Search());
                        break;

                    case 9:
                        People.TraceChildren(People.Search());
                        break;

                    case 10:
                        Console.WriteLine("Exiting...");
                        isRunning = false;
                        break;
                }
            }
        }

        #endregion Menus

        #region Tools

        //Confirms correct input used in switch
        public static int ConfirmCorrectInput(int allowedRange)
        {
            int confirmedChoice;
            do
            {
                string menuChoiceString = Console.ReadLine();

                bool successfulConversion = Int32.TryParse(menuChoiceString, out confirmedChoice);

                if (successfulConversion && confirmedChoice <= allowedRange)
                {
                    break;
                }
                else if (menuChoiceString == "")
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid input!");
                }
            } while (true);
            return confirmedChoice;
        }

        //Only allows int as input
        public static int ConfirmCorrectInput(string value)
        {
            int confirmedChoice;
            do
            {
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

        //Creates a space between screens
        public static void ContinueAndClear()
        {
            Console.WriteLine("\nPress Enter to continue");
            Console.ReadLine();
            Console.Clear();
        }

        #endregion Tools
    }
}