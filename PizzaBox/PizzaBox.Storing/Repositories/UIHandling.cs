using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Storing.Repositories
{
    public static class UIHandling
    {
        //Provides an introduction message to the user.
        public static string Intro()
        {
            Console.WriteLine("Hello! Welcome to PizzaBox, where you can order pizza.");
            Console.WriteLine("Are you a returning member? Yes (y) or No (no)");
            return Console.ReadLine();

        }

        //Provides UI for signing in.
        public static string[] SigningIn()
        {
            string[] credentials = new string[2];
            Console.WriteLine("You are signing in. Please enter your credentials.");
            Console.Write("Username: ");
            credentials[0] = Console.ReadLine();
            Console.Write("Password: ");
            credentials[1] = Console.ReadLine();
            
            return credentials;
        }

        //Provides UI for placing order.
        public static void PlacingOrder()
        {
            Console.WriteLine("You are placing an order....");
        }

        //Provides UI for the user to create an account.
        public static void CreatingAccount()
        {
            Console.WriteLine("You are creating an account....");
        }

        //Provides message informing user that their response was invalid.
        public static void InvalidResponse()
        {

        }
    }
}
