using System;
using System.Collections.Generic;
using System.Text;
using PizzaBox.Domain.Models;


namespace PizzaBox.Storing.Repositories
{
    public static class UIHandling
    {
        private static StoreDBContext db = DatabaseAccess.GetDatabase();
        //Provides an introduction message to the user.
        public static void Intro()
        {
            Console.WriteLine("Hello! Welcome to PizzaBox, where you can order pizza.");
        }

        public static bool ReturningMember()
        {
            Console.Clear();
            Console.WriteLine("Are you a returning member? Yes (y) or No (no)");
            string response = Console.ReadLine().ToLower();
            if (response == "y") //might need change to switch block
            {
                return true;
            }
            return false;
        }

        //Provides UI for signing in.
        public static void SigningIn(ref bool isAdmin, ref bool validUser, ref bool loggedIn)
        {
            Console.Clear();
            string[] credentials = new string[2];
            Console.WriteLine("You are signing in. Please enter your credentials.");
            Console.Write("Username: ");
            credentials[0] = Console.ReadLine();
            Console.Write("Password: ");
            credentials[1] = Console.ReadLine();

            isAdmin = DataHandling.IsAdmin(credentials[0], credentials[1]);
            validUser = DataHandling.IsValidUser(credentials[0], credentials[1]);

            if (validUser)
            {
                loggedIn = true;
            }
        }

        //Provides UI for the user to create an account.
        public static void CreatingAccount()
        {
            Console.Clear();
            bool canCreateAccount = false;
            
            while (!canCreateAccount)
            {
                Console.WriteLine("You are creating an account. Please enter your credentials.");
                Console.Write("First Name: ");
                string possibleFirstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string possibleLastName = Console.ReadLine();
                Console.Write("Username: ");
                string possibleUsername = Console.ReadLine();
                Console.Write("Password: ");
                string possiblePassword = Console.ReadLine();

                canCreateAccount = DataHandling.CanCreateAccount(
                    possibleFirstName, possibleLastName, possibleUsername, possiblePassword);

                if (!canCreateAccount)
                {
                    Console.WriteLine("Cannot create account. Please try a different username.");
                }
            }

            

        }

        //Provides message informing user that their response was invalid.
        public static void InvalidResponse()
        {

        }
    }
}
