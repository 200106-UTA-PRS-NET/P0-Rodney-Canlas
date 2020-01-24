using System;
using System.Collections.Generic;
using System.Text;
using PizzaBox.Domain.Models;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace PizzaBox.Storing.Repositories
{
    public static class DataHandler
    {
        private static StoreDBContext db = DatabaseAccess.GetDatabase();

        /** <summary> 
         *  Checks if the given USERNAME and PASSWORD are associated 
         *      with an account. 
         *  </summary>
         *  <returns> 
         *  Returns true if USERNAME and PASSWORD matches to an 
         *      account; otherwise, returns false.
         *  </returns>
         */
        public static bool IsValidUser(string username, string password)
        {
            if (db.Account.Any(a => a.Username.Equals(username) && a.Passphrase.Equals(password)))
            {
                return true;
            }
            return false;
        }

        /** <summary>
         *  Checks if the given USERNAME is "admin" and the PASSWORD is associated
         *      with an the USERNAME.
         *  </summary>
         *  <returns>
         *  Returns true if USERNAME is "admin" and PASSWORD is "password"; 
         *      otherwise, returns false.
         *  </returns>
         */
        public static bool IsAdmin(string username, string password)
        {
            //string userName = username.ToLower();
            if (username == "admin" && password == "password")
            {
                return true;
            }
            return false;
        }

        /** <summary>
         *  Checks if the given USERNAME already exists in the StoreDB.Account table.
         *      If it does not, it will create and update the table with a new user 
         *      with the passed-in credentials. 
         *  </summary>
         *  <returns>
         *  Returns true if the USERNAME does not exist yet; otherwise,
         *      returns false.
         *  </returns>
         */
        public static bool CanCreateAccount(string firstName, string lastName, string username, string password)
        {
            if (db.Account.Any(a => a.Username == username))
            {
                return false;
            }
            Account newUser = new Account()
            { 
                FirstName = firstName, 
                LastName = lastName, 
                Username = username, 
                Passphrase = password
            };

            db.Account.Add(newUser);
            db.SaveChanges();
            return true;

        }

        // <summary> Adds a new order to the StoreDB.UserOrder table. </summary>
        public static void AddOrder(Account currUser, int storeID, List<Pizza> orderContent, decimal totalCost)
        {
            int userID = currUser.UserId;

            //Serialize orderContent into an XML string to store in PizzaBox.UserOrder table
            string orderContentXMLString = SerializeToXMLString(orderContent);

            UserOrder newUserOrder = new UserOrder()
            {
                StoreId = storeID,
                UserId = userID,
                OrderContent = orderContentXMLString,
                TotalCost = totalCost
            };

            db.UserOrder.Add(newUserOrder);
            db.SaveChanges();
        }

        /** <summary> Gets the UserID of the given USERNAME. </summary>
         *  <returns> Returns the user's ID. </returns>
         */
        public static int GetUserID(string username)
        {
            var query = from u in db.Account
                        where u.Username == username
                        select u;

            return query.First().UserId;
        }

        /** <summary> Gets a store's name given a STOREID. </summary>
         *  <returns> Returns the store's name. </returns>
         */
        public static string GetStoreNameByID(int storeID)
        {
            if (db.Store.Any(s => s.StoreId == storeID))
            {
                return db.Store.FirstOrDefault(s => s.StoreId == storeID).StoreName;
            } else
            {
                return "";
            }
           
        }

        /** <summary> Serializes a List object, which holds Pizza objects, into a string. </summary>
         *  <returns> Returns the serialized list of pizzas. </returns>
         */
        private static string SerializeToXMLString(List<Pizza> orderContent) 
        {
            using (StringWriter strWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pizza>));
                serializer.Serialize(strWriter, orderContent);
                return strWriter.ToString();
            }
        }

        /** <summary> Deserializes a string to a List object, which holds Pizza objects. </summary>
         *  <returns> Returns a List object, which holds Pizza objects. </returns>
         */
        public static List<Pizza> DeserializeFromXMLString(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<Pizza>));

            using (TextReader reader = new StringReader(xml))
            {
                return (List<Pizza>)serializer.Deserialize(reader);
            }
        }

        /** <summary> Gets the orders that match a store's ID. </summary>
         *  <returns> Returns a set of orders. </returns>
         */
        public static IQueryable<UserOrder> GetOrdersByStoreID(int storeID)
        {
            var orders = from o in db.UserOrder
                         where o.StoreId == storeID
                         select o;

            return orders;
        }

        /** <summary> Gets the orders that match a user's ID. </summary>
         *  <returns> Returns a set of orders. </returns>
         */
        public static IQueryable<UserOrder> GetOrdersByUserID(int userID)
        {
            var orders = from o in db.UserOrder
                         where o.UserId == userID
                         select o;

            return orders;
        }

        /** <summary> 
         *  Checks if the current user can order from the store
         *      that has a matching ID. 
         *  </summary>
         *  <returns> 
         *  Returns true if the current user has not ordered
         *      from the store within 24 hours; otherwise, returns false.
         *  </returns>
         */
        public static bool CanOrderFromLocation(in Account currUser, int storeID)
        {
            int currUserID = currUser.UserId;

            var relevantOrders = GetOrdersByStoreID(storeID);

            if (relevantOrders.Any(o => o.UserId == currUserID))
            {
                var ordersByUser = from o in relevantOrders
                                   where o.UserId == currUserID
                                   select o;

                UserOrder lastOrderByUser = ordersByUser.OrderBy(o => o.OrderDateTime).Last();
                DateTime lastOrderDateTime = lastOrderByUser.OrderDateTime;

                TimeSpan orderGap = DateTime.Now - lastOrderDateTime;
               
                if (orderGap.TotalDays >= 1)
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return true;
            }
        }

        /** <summary> Gets the user given the USERNAME. </summary>
         *  <returns> Returns the user. </returns>
         */
        public static Account GetUserByUsername(string username)
        {
            int userID = GetUserID(username);

            return db.Account.Find(userID);
        }

        /** <summary> 
         *  Stores a set of stores into a Dictionary object, with 
         *      the store's ID as the key and the store's name as
         *      the value.
         *  </summary>
         *  <returns>
         *  Returns a Dictionary object, with 
         *      the store's ID as the key and the store's name as
         *      the value.
         *  </returns>
         */
        public static Dictionary<int, string> ListOfStores()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            var stores = from s in db.Store
                         select s;

            foreach (Store store in stores)
            {
                result.Add(store.StoreId, store.StoreName);
            }

            return result;
        }

        /** <summary> 
         *  Gets a set of users from StoreDB.UserOrders that 
         *      match the given STOREID.     
         *  </summary>
         *  <returns> Returns a set of users. </returns>
         */
        public static IQueryable<Account> GetUsersByStoreID(int storeID)
        {
            var orders = GetOrdersByStoreID(storeID);

            var users = from user in db.Account
                        join order in orders
                        on user.UserId equals order.UserId
                        select user;
            
            return users.Distinct();
        }
    }
}

