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

        public static bool IsValidUser(string username, string password)
        {
            if (db.Account.Any(a => a.Username == username && a.Passphrase == password))
            {
                return true;
            }
            return false;
        }

        public static bool IsAdmin(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                return true;
            }
            return false;
        }

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

        public static int GetUserID(string username)
        {
            var query = from u in db.Account
                        where u.Username == username
                        select u;

            return query.First().UserId;
        }

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

        private static string SerializeToXMLString(List<Pizza> orderContent) 
        {
            using (StringWriter strWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pizza>));
                serializer.Serialize(strWriter, orderContent);
                return strWriter.ToString();
            }
        }

        public static List<Pizza> DeserializeFromXMLString(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<Pizza>));

            using (TextReader reader = new StringReader(xml))
            {
                return (List<Pizza>)serializer.Deserialize(reader);
            }
        }

        public static IQueryable<UserOrder> GetOrdersByStoreID(int storeID)
        {
            var orders = from o in db.UserOrder
                         where o.StoreId == storeID
                         select o;

            return orders;
        }

        public static IQueryable<UserOrder> GetOrdersByUserID(int userID)
        {
            var orders = from o in db.UserOrder
                         where o.UserId == userID
                         select o;

            return orders;
        }

        public static bool CanOrderFromLocation(in Account currUser, int storeID)
        {
            int currUserID = currUser.UserId;

            var relevantOrders = GetOrdersByStoreID(storeID);

            if (relevantOrders.Any(o => o.UserId == currUserID))
            {
                var ordersByUser = from o in relevantOrders
                                   where o.UserId == currUserID
                                   select o;

                UserOrder lastOrderByUser = ordersByUser.OrderBy(o => o.UserId == currUserID).Last();

                //UserOrder lastOrderByUser = relevantOrders.OrderBy();
                DateTime lastOrderDateTime = lastOrderByUser.OrderDateTime;

                TimeSpan orderGap = DateTime.Now - lastOrderDateTime;
                
                //Console.WriteLine(orderGap.TotalDays);
                //Console.ReadLine();
               
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

        public static Account GetUserByUsername(string username)
        {
            int userID = GetUserID(username);

            return db.Account.Find(userID);
        }

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

