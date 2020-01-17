using System;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;



namespace PizzaBox.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            bool isAdmin = false;
            bool validUser = false;
            bool loggedIn = false;

            UIHandling.Intro();
            if (UIHandling.ReturningMember())
            {
                UIHandling.SigningIn(ref isAdmin, ref validUser, ref loggedIn);
            }
            else
            {
                UIHandling.CreatingAccount();
                UIHandling.SigningIn(ref isAdmin, ref validUser, ref loggedIn);
            }
            while (!validUser)
            {
                UIHandling.CreatingAccount();
                UIHandling.SigningIn(ref isAdmin, ref validUser, ref loggedIn);
            }
            if (validUser && loggedIn)
            {
                Console.WriteLine("You are logged in!");
            }
        }
    }
}
