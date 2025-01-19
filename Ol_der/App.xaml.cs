using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ol_der.Controls.Orders;
using Ol_der.Data;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Ol_der
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IConfigurationRoot Configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            Task.Run(() =>
            {
                try
                {
                    using (var context = new ApplicationDbContext(optionsBuilder.Options))
                    {
                        context.Database.EnsureCreated();
                    }
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBoxOkWindow messageBoxOkWindow = new MessageBoxOkWindow($"Nem sikerült csatlakozni az adatbázishoz");
                        messageBoxOkWindow.ShowDialog();
                    });
                }
            });
        }
    }

}
