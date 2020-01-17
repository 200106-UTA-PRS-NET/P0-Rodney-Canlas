using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaBox.Domain.Models;

namespace PizzaBox.Storing.Repositories
{
    public sealed class DatabaseAccess //Singleton Object
    {
        private static readonly DatabaseAccess instance = new DatabaseAccess();
        private static DbContextOptions<StoreDBContext> options;

        static DatabaseAccess() { }
        private DatabaseAccess()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<StoreDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("StoreDB"));
            options = optionsBuilder.Options;
        }
        public static DatabaseAccess Instance
        {
            get
            {
                return instance;
            }
        }

        public static StoreDBContext GetDatabase()
        {
            return new StoreDBContext(options);
        }
    }
}
