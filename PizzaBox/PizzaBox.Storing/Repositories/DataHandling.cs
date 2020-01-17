using System;
using System.Collections.Generic;
using System.Text;
using PizzaBox.Domain.Models;
using System.Linq;

namespace PizzaBox.Storing.Repositories
{
    public static class DataHandling
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
            { FirstName = firstName, LastName = lastName, Username = username, Passphrase = password};
            db.Account.Add(newUser);
            db.SaveChanges();
            return true;

        }
    }
}
