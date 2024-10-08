using HealthApp.Database;
using System;

namespace HealthApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using (var context = new DatabaseSource())
            {
                context.InitializeDatabase();
            }

            MainPage = new AppShell();
        }
    }
}
