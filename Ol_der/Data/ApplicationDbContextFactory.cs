﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using Ol_der.Controls.Orders;

namespace Ol_der.Data
{
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return CreateApplicationDbContext();
        }

        public static ApplicationDbContext Create()
        {
            return CreateApplicationDbContext();
        }

        private static ApplicationDbContext CreateApplicationDbContext()
        {
           
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            var context = new ApplicationDbContext(builder.Options);

            if (!context.Database.CanConnect())
            {
                MessageBoxOkWindow messageBoxOkWindow0 = new MessageBoxOkWindow("Nincs adatbázis kapcsolat");
                messageBoxOkWindow0.ShowDialog();
            }

            return context;

        }
    }
}
