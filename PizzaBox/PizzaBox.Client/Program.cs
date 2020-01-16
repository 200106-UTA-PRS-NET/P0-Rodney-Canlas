using System;
using System.Collections.Generic;
using System.Text.Json;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                string returning = UIHandling.Intro();
                switch (returning)
                {
                    case "y":
                        Console.Clear();
                        
                        string[] userCredentials = UIHandling.SigningIn();
                        bool isValidUser = DataHandling.IsValidUser(userCredentials[0], userCredentials[1]);
                        
                        if (isValidUser)
                        {
                            UIHandling.PlacingOrder();
                        } else
                        {
                            UIHandling.CreatingAccount();
                        }
                        break;
                    case "n":
                        Console.Clear();
                        UIHandling.CreatingAccount();
                        break;
                    default:
                        Console.Clear();
                        UIHandling.InvalidResponse();
                        break;

                } 
            }


        }
    }
}
