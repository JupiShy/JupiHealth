using HealthApp.Database;
using System;

namespace HealthApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            using (var db = new DatabaseSource())
            {
                db.InitializeDatabase();
            }

            MainPage = new AppShell();
        }
    }
}
