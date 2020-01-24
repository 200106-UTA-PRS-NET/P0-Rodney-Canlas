using System;
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
                bool isAdmin = false;
                bool validUser = false;
                bool loggedIn = false;
                string username = "";

                if (UIHandler.ReturningUser()) 
                { 
                    UIHandler.SigningIn(ref username, ref isAdmin, ref validUser, ref loggedIn);
                }
                else
                {
                    UIHandler.CreatingAccount();
                    UIHandler.SigningIn(ref username, ref isAdmin, ref validUser, ref loggedIn);
                }
                while (!validUser)
                {
                    bool createAccount = UIHandler.WantToCreateAccount();
                    if (createAccount)
                    {
                        UIHandler.CreatingAccount();
                        UIHandler.SigningIn(ref username, ref isAdmin, ref validUser, ref loggedIn);
                    } else
                    {
                        UIHandler.SigningIn(ref username, ref isAdmin, ref validUser, ref loggedIn);
                    }
                }


                while (validUser && loggedIn)
                {
                    Account currUser = DataHandler.GetUserByUsername(username);
                    username = currUser.Username;
                    bool isMakingOrder = false;
                    if (isAdmin)
                    {
                        string adminInput = UIHandler.AdminMenu(in currUser);
                        switch (adminInput)
                        {
                            case "1":
                                isMakingOrder = true;
                                UIHandler.MakingOrder(in currUser, in isMakingOrder);
                                break;
                            case "2":
                                isMakingOrder = false;
                                UIHandler.AccessingStoreData(in currUser, in isMakingOrder);
                                break;
                            case "3":
                                isMakingOrder = false;
                                UIHandler.ViewingOrderHistory(in currUser);
                                break;
                            case "0":
                                UIHandler.SigningOut(ref isAdmin, ref validUser, ref loggedIn);
                                break;
                        }
                    }
                    else
                    {
                        string userInput = UIHandler.UserMenu(currUser);
                        switch (userInput)
                        {
                            case "1":
                                isMakingOrder = true;
                                UIHandler.MakingOrder(in currUser, in isMakingOrder);
                                break;
                            case "2":
                                isMakingOrder = false;
                                UIHandler.ViewingOrderHistory(in currUser);
                                break;
                            case "0":
                                UIHandler.SigningOut(ref isAdmin, ref validUser, ref loggedIn);
                                break;
                        }
                    }
                }
            }


        }
    }
}
