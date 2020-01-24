using System;
using System.Collections.Generic;
using System.Text;
using PizzaBox.Domain.Models;
using Xunit;

namespace PizzaBox.Testing.Library.Models
{
    public class AccountTest
    {
        private readonly Account _account = new Account();

        [Fact]
        public void FirstName_NonEmptyValue_StoresCorrectly()
        {
            string randomNameValue = "Esperanza";
            _account.FirstName = randomNameValue;
            Assert.Equal(randomNameValue, _account.FirstName);
        }

        [Fact]
        public void Username_NonEmptyValue_StoresCorrectly()
        {
            string randomUsernameValue = "volleygirl123";
            _account.Username = randomUsernameValue;
            Assert.Equal(randomUsernameValue, _account.Username);
        }

        [Fact]
        public void UserID_NonEmptyValue_StoresCorrectly()
        {
            int randomUserIDValue = 1;
            _account.UserId = randomUserIDValue;
            Assert.Equal(randomUserIDValue, _account.UserId);
        }
    }
}
