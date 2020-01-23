using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PizzaBox.Domain.Models;


namespace PizzaBox.Storing.Repositories
{
    public static class UIHandler
    {
        //Provides an introduction message to the user.
        public static void Intro()
        {
            Console.WriteLine("Hello! Welcome to PizzaBox, where you can order pizza.");
        }

        public static bool ReturningUser()
        {
            bool validResponse = false;
            bool returningUser = false;
            while (!validResponse)
            {
                Intro();
                Console.WriteLine("Are you a returning member? Yes (y) or No (n)");
                
                string response = Console.ReadLine().ToLower();
                switch (response)
                {
                    case "y":
                        validResponse = true;
                        returningUser = true;
                        break;
                    case "n":
                        validResponse = true;
                        returningUser = false;
                        break;
                    default:
                        InvalidResponse();
                        break;
                }
            }
            return returningUser;
        }

        //Provides UI for the user to create an account.
        public static void CreatingAccount()
        {
            bool canCreateAccount = false;

            while (!canCreateAccount)
            {
                Console.Clear();
                Console.WriteLine("You are creating an account. Please enter your credentials.");
                Console.Write("First Name: ");
                string possibleFirstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string possibleLastName = Console.ReadLine();
                Console.Write("Username: ");
                string possibleUsername = Console.ReadLine();
                Console.Write("Password: ");
                string possiblePassword = Console.ReadLine();

                canCreateAccount = DataHandler.CanCreateAccount(
                    possibleFirstName, possibleLastName, possibleUsername, possiblePassword);

                if (!canCreateAccount)
                {
                    Console.WriteLine("Cannot create account. Please try a different username.");
                    Console.ReadLine();
                }
            }
        }

        //Provides UI for signing in.
        public static void SigningIn(ref string username, ref bool isAdmin, ref bool validUser, ref bool loggedIn)
        {
            Console.Clear();
            string[] credentials = new string[2];
            Console.WriteLine("You are signing in. Please enter your credentials.");
            Console.Write("Username: ");
            credentials[0] = Console.ReadLine();
            username = credentials[0];
            Console.Write("Password: ");
            credentials[1] = Console.ReadLine();

            isAdmin = DataHandler.IsAdmin(credentials[0], credentials[1]);
            
            if (!isAdmin)
            {
                validUser = DataHandler.IsValidUser(credentials[0], credentials[1]);
            } else
            {
                validUser = true;
            }

            if (validUser)
            {
                loggedIn = true;
            }
        }
        
        public static void SigningOut(ref bool isAdmin, ref bool validUser, ref bool loggedIn)
        {
            isAdmin = false;
            validUser = false;
            loggedIn = false;
            Console.Clear();
            Console.WriteLine("You are signed out.");
            Console.ReadLine();
        }

        public static string AdminMenu(in Account currUser)
        {
            Console.Clear();

            string userInput = "invalid"; //userInput initialized as an invalid input
            string[] inputOptions = new string[] { "0", "1", "2", "3" };
            HashSet<string> validInputs = ValidInputs(inputOptions);
            
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine($"Welcome to the ADMIN menu, {currUser.Username}!");
                Console.WriteLine("1. Make an order.");
                Console.WriteLine("2. Access store data.");
                Console.WriteLine("3. View order history.");
                Console.WriteLine("0. Sign out.");
                Console.Write("\nWhat would you like to do? ");

                userInput = Console.ReadLine();
                if (!validInputs.Contains(userInput))
                {
                    InvalidResponse();
                } else
                {
                    validInput = true;
                }
            }
            return userInput;
        }

        public static string UserMenu(Account currUser)
        {
            Console.Clear();

            bool validInput = false;
            string userInput = "invalid"; //userInput initialized as an invalid input
            string[] inputOptions = new string[] { "0", "1", "2" };
            HashSet<string> validInputs = ValidInputs(inputOptions);

            while (!validInput)
            {
                Console.WriteLine($"Welcome to the USER menu, {currUser.Username}!");
                Console.WriteLine("1. Make an order.");
                Console.WriteLine("2. View order history.");
                Console.WriteLine("0. Sign out.");
                Console.Write("\nWhat would you like to do? ");

                userInput = Console.ReadLine();
                if (!validInputs.Contains(userInput))
                {
                    InvalidResponse();
                    Console.Read();
                } else
                {
                    validInput = true;
                }
            }
            return userInput;
        }

        private static HashSet<string> ValidInputs(string[] validInputs)
        {
            HashSet<string> inputs = new HashSet<string>();
            foreach (string number in validInputs)
            {
                inputs.Add(number);
            }
            return inputs;
        }

        public static void MakingOrder(in Account currUser, in bool isMakingOrder)
        {
            Console.Clear();
            Console.WriteLine("You are making an order! Remember that orders that exceed $250 will NOT be confirmed.");
            Console.ReadLine();

            decimal totalCost;
            bool doneOrdering = false;
            string pizzaType = "";
            List<Pizza> orderContent = new List<Pizza>();

            int storeID = ChoosingLocation(in currUser, in isMakingOrder);
            if (storeID == 0)
            {
                doneOrdering = true;
            }
            else
            {
                pizzaType = ChoosingPizzaType();
            }

            while (!doneOrdering)
            {
                bool validResponse = false;
                while (!validResponse)
                {
                    switch (pizzaType)
                    {
                        case "1":
                            validResponse = true;

                            int count = 0;
                            Pizza preset = DesigningPresetPizza(ref count);
                            StartingOrder(preset, orderContent, count);

                            bool validResponse1 = false;
                            while (!validResponse1)
                            {
                                totalCost = TotalCost(orderContent);
                                string userInput = Deciding(orderContent, totalCost);
                                switch (userInput)
                                {
                                    case "1": //Add to order.
                                        if (totalCost > 250)
                                        {
                                            validResponse1 = false;
                                            doneOrdering = false;
                                            CostsLimitExceeded();
                                        }
                                        else
                                        {
                                            validResponse1 = true;
                                            doneOrdering = false;
                                            AddingToOrder();
                                        }
                                        break;
                                    case "2": //Confirm order.                                      
                                        if (totalCost > 250)
                                        {
                                            validResponse1 = false;
                                            doneOrdering = false;
                                            CostsLimitExceeded();
                                        } else
                                        {
                                            validResponse1 = true;
                                            doneOrdering = true;
                                            ConfirmingOrder(currUser, storeID, ref orderContent, ref totalCost);
                                        }                                        
                                        break;
                                    case "3": //Delete order.
                                        validResponse1 = true;
                                        doneOrdering = true;
                                        DeletingOrder(ref orderContent, ref totalCost);
                                        break;
                                    default:
                                        validResponse1 = false;
                                        InvalidResponse();
                                        break;
                                }
                            }

                            break;
                        case "2":
                            validResponse = true;

                            int count2 = 0;
                            Pizza custom = DesigningCustomPizza(ref count2);
                            StartingOrder(custom, orderContent, count2);

                            bool validResponse2 = false;
                            while (!validResponse2)
                            {
                                totalCost = TotalCost(orderContent);
                                string userInput = Deciding(orderContent, totalCost);
                                switch (userInput)
                                {
                                    case "1": //Add to order.
                                        if (totalCost > 250)
                                        {
                                            validResponse2 = false;
                                            doneOrdering = false;
                                            CostsLimitExceeded();
                                        } else
                                        {
                                            validResponse2 = true;
                                            doneOrdering = false;
                                            AddingToOrder();
                                        }
                                        break;
                                    case "2": //Confirm order.
                                        if (totalCost > 250)
                                        {
                                            validResponse2 = false;
                                            doneOrdering = false;
                                            CostsLimitExceeded();
                                        }
                                        else
                                        {
                                            validResponse2 = true;
                                            doneOrdering = true;
                                            ConfirmingOrder(currUser, storeID, ref orderContent, ref totalCost);
                                        }
                                        break;
                                    case "3": //Delete order.
                                        validResponse2 = true;
                                        doneOrdering = true;
                                        DeletingOrder(ref orderContent, ref totalCost);
                                        break;
                                    default:
                                        validResponse2 = false;
                                        InvalidResponse();
                                        break;
                                }
                            }

                            break;
                        default:
                            validResponse = false;
                            InvalidResponse();
                            break;
                    }
                }
            }
        }

        private static void CostsLimitExceeded()
        {
            Console.Clear();
            Console.WriteLine("Your order costs has exceeded $250. Please delete this order and make another one.");
            Console.ReadLine();
        }

        private static void DeletingOrder(ref List<Pizza> orderContent, ref decimal totalCost)
        {
            orderContent = null;
            totalCost = 0;
            Console.Clear();
            Console.WriteLine("Order deleted.");
            Console.ReadLine();
        }

        private static void ConfirmingOrder(Account currUser, int storeID, ref List<Pizza> orderContent, ref decimal totalCost)
        {
            if (totalCost > 0) {
                DataHandler.AddOrder(currUser, storeID, orderContent, totalCost);

                Console.Clear();
                Console.WriteLine("Order confirmed.");
                Console.ReadLine();

                orderContent = new List<Pizza>();
                totalCost = 0;
            } else
            {
                Console.WriteLine("Order cannot be confirmed. You did not order anything.");
                Console.ReadLine();
            }
        }

        private static void AddingToOrder()
        {
            Console.Clear();
            Console.WriteLine("You are adding to your order.");
            Console.ReadLine();
        }

        private static decimal TotalCost(List<Pizza> orderContent)
        {
            decimal totalCost = 0;
            foreach (Pizza pizza in orderContent)
            {
                switch (pizza.Size)
                {
                    case "S":
                        totalCost += 8;
                        break;
                    case "M":
                        totalCost += 10;
                        break;
                    case "L":
                        totalCost += 12;
                        break;
                }
            }
            return totalCost;
        }

        private static string Deciding(List<Pizza> orderContent, decimal totalCost)
        {
            Console.Clear();
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("Current order: ");
                int i = 1;
                foreach (Pizza pizza in orderContent)
                {
                    Console.WriteLine($"{i}. {pizza.Size} {pizza.Crust} Crust {ConvertListToppingsToString(pizza.Toppings)}Pizza");
                    i += 1;
                }
                Console.WriteLine($"\nTotal Cost: ${totalCost}");
                Console.WriteLine("\n\n-----Next Steps-----");
                Console.WriteLine("1. Add to order.");
                Console.WriteLine("2. Confirm order.");
                Console.WriteLine("3. Delete order.");
                Console.Write("\nWhat would you like to do? ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        validInput = true;
                        return userInput;
                    case "2":
                        validInput = true;
                        return userInput;
                    case "3":
                        validInput = true;
                        return userInput;
                    default:
                        validInput = false;
                        InvalidResponse();
                        break;
                }
            }
            return "";
        }

        private static string ConvertListToppingsToString(List<string> toppings)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string topping in toppings)
            {
                stringBuilder.Append(topping);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        private static List<Pizza> StartingOrder(Pizza pizza, List<Pizza> orderContent, int count)
        {
            for (int i = 0; i < count; i++)
            {
                orderContent.Add(pizza);
            }
            return orderContent;
        }

        private static int ChoosingLocation(in Account currUser, in bool isMakingOrder)
        {
            bool validResponse = false;
            while (!validResponse)
            {
                Console.Clear();
                Console.WriteLine("-----Pizza Stores-----");
                Console.WriteLine("1. Giant Caesars");
                Console.WriteLine("2. Mama Johns");
                Console.WriteLine("3. Pizza Igloo");
                Console.WriteLine("\n0. Go back to main menu.");
                Console.Write("\nPlease choose a store to order from: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        if (isMakingOrder)
                        {
                            if (CanOrderFromLocation(in currUser, userInput))
                            {
                                validResponse = true;
                                return 1;
                            }
                            else
                            {
                                CannotOrderFromLocation();
                            }
                        } else
                        {
                            validResponse = true;
                            return 1;
                        }
                        break;
                    case "2":
                        if (isMakingOrder)
                        {
                            if (CanOrderFromLocation(in currUser, userInput))
                            {
                                validResponse = true;
                                return 2;
                            }
                            else
                            {
                                CannotOrderFromLocation();
                            }
                        } else
                        {
                            validResponse = true;
                            return 2;
                        }
                        break;
                    case "3":
                        if (isMakingOrder)
                        {
                            if (CanOrderFromLocation(in currUser, userInput))
                            {
                                validResponse = true;
                                return 3;
                            }
                            else
                            {
                                CannotOrderFromLocation();
                            }
                        } else
                        {
                            validResponse = true;
                            return 3;
                        }
                        break;
                    case "0":
                        return 0;
                    default:
                        validResponse = false;
                        InvalidResponse();
                        break;
                }
            }
            return 0;
        }

        private static bool CanOrderFromLocation(in Account currUser, string userInput)
        {
            int storeID = ToDigits(userInput);
            return DataHandler.CanOrderFromLocation(in currUser, storeID);
        }

        private static void CannotOrderFromLocation()
        {
            Console.Clear();
            Console.WriteLine("You cannot order from the same location within 24 hours.");
            Console.ReadLine();
        }

        private static string ChoosingPizzaType()
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("-----Pizzas-----");
                Console.WriteLine("1. Preset Pizza");
                Console.WriteLine("2. Custom Pizza");
                Console.Write("\nPlease choose the type of pizza you want: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        validInput = true;
                        return "1";
                    case "2":
                        validInput = true;
                        return "2";
                    default:
                        validInput = false;
                        InvalidResponse();
                        break;
                }
            }
            return "";
        }

        private static Pizza DesigningPresetPizza(ref int count)
        {
            Console.Clear();

            Pizza presetPizza;
            string pizzaType = "P";
            string crust = "";
            List<string> toppings = new List<string>();
            string size = "";

            bool validInput = false;
            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your preset pizza.");
                Console.WriteLine("\n-----Preset Pizzas-----");
                Console.WriteLine("1. Cheese Pizza");
                Console.WriteLine("2. Veggie Pizza");
                Console.WriteLine("3. Pepperoni Pizza");
                Console.Write("\nPlease choose a preset pizza: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        validInput = true;
                        toppings.Add("Cheese");
                        break;
                    case "2":
                        validInput = true;
                        toppings.Add("Veggies");
                        break;
                    case "3":
                        validInput = true;
                        toppings.Add("Pepperoni");                        
                        break;
                    default:
                        validInput = false;
                        InvalidResponse();
                        break;
                }
            }

            bool validInput1 = false;
            while (!validInput1)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your preset pizza.");
                Console.WriteLine("\n-----Types of Crust-----");
                Console.WriteLine("1. Thin");
                Console.WriteLine("2. Thick");
                Console.Write("\nPlease choose a crust: ");
                string userInput1 = Console.ReadLine();

                switch (userInput1)
                {
                    case "1":
                        validInput1 = true;
                        crust = "Thin";
                        break;
                    case "2":
                        validInput1 = true;
                        crust = "Thick";
                        break;
                    default:
                        validInput1 = false;
                        InvalidResponse();
                        break;
                }
            }

            bool validInput2 = false;
            while (!validInput2)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your preset pizza.");
                Console.WriteLine("\n-----Size-----");
                Console.WriteLine("1. Small  - $8.00");
                Console.WriteLine("2. Medium - $10.00");
                Console.WriteLine("3. Large  - $12.00");
                Console.Write("\nPlease choose a size: ");
                string userInput2 = Console.ReadLine();

                switch (userInput2)
                {
                    case "1":
                        validInput2 = true;
                        size = "S";
                        break;
                    case "2":
                        validInput2 = true;
                        size = "M";
                        break;
                    case "3":
                        validInput2 = true;
                        size = "L";
                        break;
                    default:
                        validInput2 = false;
                        InvalidResponse();
                        break;
                }
            }

            bool validInput3 = false;
            while (!validInput3)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your preset pizza.");
                Console.Write("\nHow many of this pizza do you want? ");
                string countInput = Console.ReadLine();

                if (!IsDigitsOnly(countInput) || (countInput.Length >= 8))
                {
                    validInput3 = false;
                    InvalidResponse();
                } else
                {
                    if (ToDigits(countInput) > 100)
                    {
                        validInput3 = false;
                        PizzaLimitExceeded();
                    } else
                    {
                        validInput3 = true;
                        count = ToDigits(countInput);
                    }
                }
            }

            presetPizza = new Pizza()
            {
                PizzaType = pizzaType,
                Crust = crust,
                Size = size,
                Toppings = toppings,
            };

            return presetPizza;
        }

        private static Pizza DesigningCustomPizza(ref int count)
        {
            Console.Clear();

            Pizza customPizza;
            string pizzaType = "C";
            string crust = "";
            List<string> toppings = new List<string>();
            string size = "";

            bool validInput = false;
            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your custom pizza.");
                Console.WriteLine("\n-----Types of Crust-----");
                Console.WriteLine("1. Thin");
                Console.WriteLine("2. Thick");
                Console.Write("\nPlease choose a crust: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        validInput = true;
                        crust = "Thin";
                        break;
                    case "2":
                        validInput = true;
                        crust = "Thick";
                        break;
                    default:
                        validInput = false;
                        InvalidResponse();
                        break;
                }
            }

            bool validInput1 = false;
            while (!validInput1)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your custom pizza.");
                Console.WriteLine("\n-----Toppings-----");
                Console.WriteLine("0. Pepperoni");
                Console.WriteLine("1. Bacon");
                Console.WriteLine("2. Sausage");
                Console.WriteLine("3. Chicken");
                Console.WriteLine("4. Anchovies");
                Console.WriteLine("5. Veggies");
                Console.WriteLine("6. Spinach");
                Console.WriteLine("7. Pineapple");
                Console.WriteLine("8. Jalapeno");
                Console.WriteLine("9. Cheese");

                Console.Write("\nPlease choose up to 5 toppings: ");
                string userInput1 = Console.ReadLine();
                userInput1 = userInput1.Replace(" ", "");
                int numOfToppings = userInput1.Length;

                if (IsDigitsOnly(userInput1))
                {
                    if (numOfToppings > 5)
                    {
                        validInput1 = false;
                        NumberOfToppingsExceeded();
                    } else
                    {
                        validInput1 = true;
                        AddToppings(toppings, userInput1);
                    }
                }
                else
                {
                    validInput1 = false;
                    InvalidResponse();
                }
            }

            bool validInput2 = false;
            while (!validInput2)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your custom pizza.");
                Console.WriteLine("\n-----Size-----");
                Console.WriteLine("1. Small  - $8.00");
                Console.WriteLine("2. Medium - $10.00");
                Console.WriteLine("3. Large  - $12.00");
                Console.Write("\nPlease choose a size: ");
                string userInput2 = Console.ReadLine();

                switch (userInput2)
                {
                    case "1":
                        validInput2 = true;
                        size = "S";
                        break;
                    case "2":
                        validInput2 = true;
                        size = "M";
                        break;
                    case "3":
                        validInput2 = true;
                        size = "L";
                        break;
                    default:
                        validInput2 = false;
                        InvalidResponse();
                        break;
                }
            }

            bool validInput3 = false;
            while (!validInput3)
            {
                Console.Clear();
                Console.WriteLine("You are making an order. Please design your custom pizza.");
                Console.Write("\nHow many of this pizza do you want? ");
                string countInput = Console.ReadLine();

                if (!IsDigitsOnly(countInput))
                {
                    validInput3 = false;
                    InvalidResponse();
                }
                else
                {
                    if (ToDigits(countInput) > 100)
                    {
                        validInput3 = false;
                        PizzaLimitExceeded();
                    }
                    else
                    {
                        validInput3 = true;
                        count = ToDigits(countInput);
                    }
                }
            }

            customPizza = new Pizza()
            {
                PizzaType = pizzaType,
                Crust = crust,
                Size = size,
                Toppings = toppings,
            };

            return customPizza;

        }

        private static void AddToppings(List<string> toppings, string userInput)
        {
            Dictionary<string, string> numberToTopping = new Dictionary<string, string>()
            {
                { "0", "Pepperoni" },
                { "1", "Bacon" },
                { "2", "Sausage" },
                { "3", "Chicken" },
                { "4", "Anchovies" },
                { "5", "Veggies" },
                { "6", "Spinach" },
                { "7", "Pineapple" },
                { "8", "Jalapeno" },
                { "9", "Cheese" },
            };

            foreach (char input in userInput)
            {
                toppings.Add(numberToTopping[input.ToString()]);
            }
        }

        private static void NumberOfToppingsExceeded()
        {
            Console.Clear();
            Console.WriteLine("You cannot order more than 5 toppings.");
            Console.ReadLine();
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }

        //Converts string STR to an int, assuming that the string contains numerical digits
        private static int ToDigits(string str)
        {
            return Convert.ToInt32(str);
        }

        private static void PizzaLimitExceeded()
        {
            Console.Clear();
            Console.WriteLine("You cannot order more than 100 pizzas.");
            Console.ReadLine();
        }

        public static void AccessingStoreData(in Account currUser, in bool isMakingOrder)
        {
            Console.Clear();
            Console.WriteLine("You are accessing store data.");
            Console.ReadLine();

            bool validInput = false;

            int storeID = ChoosingLocation(in currUser, in isMakingOrder);
            if (storeID == 0)
            {
                validInput = true;
            }

            
            while (!validInput)
            {
                string storeName = DataHandler.GetStoreNameByID(storeID);
                Console.Clear();
                Console.WriteLine("-----Actions-----");
                Console.WriteLine("1. View completed orders.");
                Console.WriteLine("2. View sales.");
                Console.WriteLine("3. View inventory.");
                Console.WriteLine("4. View users.");
                Console.Write($"\nPlease choose an action for {storeName}: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        validInput = true;
                        ViewCompletedOrders(storeID, storeName);
                        break;
                    case "2":
                        validInput = true;
                        ViewSales(storeID, storeName);
                        break;
                    case "3":
                        validInput = true;
                        ViewInventory(storeID, storeName);
                        break;
                    case "4":
                        validInput = true;
                        ViewUsers(storeID, storeName);
                        break;
                    default:
                        InvalidResponse();
                        break;
                }
            }
        }

        private static void ViewCompletedOrders(int storeID, string storeName)
        {
            var completedOrders = DataHandler.GetOrdersByStoreID(storeID);

            Console.Clear();
            Console.WriteLine($"Viewing completed order for {storeName}.");
            Console.WriteLine("");
            Console.WriteLine("   Order  UserID  Pizzas   Cost       Date/Time");
            Console.WriteLine("-----------------------------------------------------");
            
            int i = 1;
            foreach (UserOrder order in completedOrders)
            {
                string orderContentStr = order.OrderContent;
                List<Pizza> orderContent = DataHandler.DeserializeFromXMLString(orderContentStr);
                int numOfPizza = orderContent.Count;

                string m = "$";
                string totalCost = String.Format("{0:0.00}", order.TotalCost);
                string orderID = String.Format("{0, -4}", order.OrderId);
                string userID = String.Format("{0, -5}", order.UserId);
                string numPizzas = String.Format("{0, -5}", numOfPizza);
                string cost = String.Format("{0, -5}", totalCost);
                string dateTime = String.Format("{0, -5}", order.OrderDateTime);
                Console.WriteLine($"{i}.  #{orderID}   {userID}  {numPizzas} {m}{cost}    {dateTime}");

                i += 1;
            }
            Console.Write("\nPlease press Enter when finished viewing. ");
            Console.ReadLine();
        }

        private static void ViewSales(int storeID, string storeName)
        {
            Console.Clear();
            Console.WriteLine("You are viewing sales.");
            Console.WriteLine();

            var completedOrders = DataHandler.GetOrdersByStoreID(storeID);
            Dictionary<string, int> pizzaSalesCounter = new Dictionary<string, int>();
            Dictionary<string, int> toppingSalesCounter = new Dictionary<string, int>();

            foreach (UserOrder order in completedOrders)
            {
                List<Pizza> contentOrder = DataHandler.DeserializeFromXMLString(order.OrderContent);
                
                foreach (Pizza pizza in contentOrder)
                {
                    string pizzaItem = $"{pizza.Size} {pizza.Crust} Crust Pizza";
                    if (pizzaSalesCounter.ContainsKey(pizzaItem))
                    {
                        pizzaSalesCounter[pizzaItem] += 1;
                    } else
                    {
                        pizzaSalesCounter.Add(pizzaItem, 1);
                    }

                    foreach (string topping in pizza.Toppings)
                    {
                        string toppingItem = $"{topping} Topping";
                        if (toppingSalesCounter.ContainsKey(toppingItem))
                        {
                            toppingSalesCounter[toppingItem] += 1;
                        }
                        else
                        {
                            toppingSalesCounter.Add(toppingItem, 1);
                        }
                    }
                }
            }

            Console.WriteLine("------------Pizzas------------");
            Console.WriteLine(" Amount Sold       Item");

            foreach (string item in pizzaSalesCounter.Keys)
            {
                string amount = String.Format("{0, -5}", pizzaSalesCounter[item]);
                string thing = String.Format("{0, 8}", item);
                Console.WriteLine($"     {amount}    {thing}");
            }

            Console.WriteLine("\n------------Toppings------------");
            Console.WriteLine(" Amount Sold       Item");

            foreach (string item in toppingSalesCounter.Keys)
            {
                string amount = String.Format("{0, -5}", toppingSalesCounter[item]);
                string thing = String.Format("{0, 8}", item);
                Console.WriteLine($"     {amount}    {thing}");
            }

            Console.Write("\nPlease press Enter when finished viewing. ");
            Console.ReadLine();
        }

        private static void ViewUsers(int storeID, string storeName)
        {
            //var users = DataHandler.GetUsersByStoreID(storeID);
            Console.Clear();
            Console.WriteLine($"You are viewing users for {storeName}.");
            Console.ReadLine();

            IQueryable<Account> users = DataHandler.GetUsersByStoreID(storeID);

            Console.WriteLine("-------Users-------");
            int i = 1;
            foreach (Account user in users)
            {
                string firstName = user.FirstName.ToLower();
                string lastName = user.LastName.ToLower();
                string fullName = $"{firstName} {lastName}";
                Console.WriteLine($"{i}. {fullName} ({user.Username})");
        
                i += 1;
            }

            Console.Write("\nPlease press Enter when finished viewing. ");
            Console.ReadLine();
        }

        private static void ViewInventory(int storeID, string storeName)
        {
            Console.Clear();
            Console.WriteLine("You are viewing inventory.");
            Console.ReadLine();
        }

        public static void ViewingOrderHistory(in Account currUser)
        { 
            int userID = currUser.UserId;
            Dictionary<int, string> storeIDToName = DataHandler.ListOfStores();
            IQueryable<UserOrder> orderHistory = DataHandler.GetOrdersByUserID(userID);

            if (orderHistory.Count() == 0)
            {
                Console.Clear();
                Console.WriteLine("You currently don't have any orders.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You are viewing your order history.");

                int i = 1;
                //string store;
                foreach (UserOrder order in orderHistory)
                {
                    //store = DataHandler.GetStoreNameByID(order.StoreId);
                    string m = "$";
                    string totalCost = String.Format("{0:0.00}", order.TotalCost);
                    int storeID = order.StoreId;

                    string orderContentStr = order.OrderContent;
                    List<Pizza> orderContent = DataHandler.DeserializeFromXMLString(orderContentStr);

                    Console.WriteLine($"\n{i}. From {storeIDToName[storeID]} on {order.OrderDateTime} | Total Cost - {m}{totalCost} ");
                    Console.WriteLine("Order Content: ");

                    foreach (Pizza pizza in orderContent)
                    {
                        Console.WriteLine($" [{pizza.PizzaType}] - {pizza.Size} {pizza.Crust} Crust {ConvertListToppingsToString(pizza.Toppings)}Pizza");
                    }

                    Console.WriteLine("\n-------------------------------------------------------");
                    i += 1;
                }
            }
            Console.Write("\nPlease press Enter when finished viewing. ");
            Console.ReadLine();

        }

        //Provides message informing user that their response was invalid.
        public static void InvalidResponse()
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Please try again.");
            Console.ReadLine();
        }
    }
}
