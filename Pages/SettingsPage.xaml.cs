using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace HealthApp
{
    public partial class SettingsPage : ContentPage
    {
        public readonly DatabaseSource db = new DatabaseSource();
        public SettingsPage()
        {
            InitializeComponent();
        }

        public async void DeleteInfoClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Очищення", "Очистити інформацію? Всі ваші записи буде втрачено назвжди", "OK", "Ні, не треба.");

            if (answer)
            {
                db.user.RemoveRange(db.user);
                db.metrics.RemoveRange(db.metrics);
                db.medicines.RemoveRange(db.medicines);
                db.medication_schedules.RemoveRange(db.medication_schedules);

                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'user'");
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'metrics'");
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'medicines'");
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'medication_schedules'");

                await db.SaveChangesAsync();     
            }
        }

        private async void AboutButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Про додаток", "Додаток розроблено Киричок Софією в межах курсового проєкту з ПАІС в ППФК", "ОК");
        }

        private async void SettingsButtonClicked(object sender, EventArgs e)
        {
            string inputName = await DisplayPromptAsync("Налаштування профілю", "Введіть Ваше ім'я:");
            await Task.Delay(100);

            int inputAge;
            string inputAgeString = await DisplayPromptAsync("Налаштування профілю", "Введіть Ваш вік:");
            int.TryParse(inputAgeString, out inputAge);
            
            var databaseHandler = new DatabaseHandler();

            if(inputName != null) await databaseHandler.ChangeUserInfo(inputName, inputAge);

            await db.SaveChangesAsync();
        }
    }
}
