using HealthApp.Database;
using Microsoft.EntityFrameworkCore;

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
            bool answer = await DisplayAlert("Очищення", "Очистити інформацію? Всі ваші записи буде втрачено назвжди", "Так", "Ні, не треба.");
            if (answer)
            {
                bool answer2 = await DisplayAlert("Очищення", "Ви впевнені?", "OK", "Назад");

                if (answer2)
                {
                    db.user.RemoveRange(db.user);
                    db.metrics.RemoveRange(db.metrics);
                    db.medicines.RemoveRange(db.medicines);
                    db.todays_schedule.RemoveRange(db.todays_schedule);
                    db.medicines_progress.RemoveRange(db.medicines_progress);

                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'user'");
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'metrics'");
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'medicines'");
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'todays_schedule'");
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'medicines_progress'");

                    await db.SaveChangesAsync();
                }
            }
        }

        private async void AboutButtonClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert(null, "Цей додаток було розроблено JupiShy в межах курсового проєкту з ПАІС в ППФК " +
                "з метою зробити процес лікування приємніше та простіше 💜",
                "Репозиторій GitHub", "ОК");

            if (result)
            {
                await Browser.Default.OpenAsync("https://github.com/JupiShy/JupiHealth", BrowserLaunchMode.SystemPreferred);
            }
        }

        private async void SettingsButtonClicked(object sender, EventArgs e)
        {
            var databaseHandler = new DatabaseHandler();
            var action = await DisplayActionSheet($"Профіль", "Вийти", null, "Мої дані", "Змінити ім'я", "Змінити вік");
            switch (action)
            {
                case "Мої дані":
                    {
                        var user = await databaseHandler.GetUserInfo();
                        if (user != null)
                        {
                            await DisplayAlert("Ваші дані", $"Ім'я: {user.name}\nВік: {user.age}", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Помилка", "Не вдалося отримати інформацію про користувача, спробуйте заповнити профіль", "OK");
                        }
                    }
                    break;
                case "Змінити ім'я":
                    {
                        string inputName = await DisplayPromptAsync("Налаштування профілю", "Введіть Ваше ім'я (до 15 символів):", maxLength:15);
                        if (inputName != null && inputName.Length <= 15)
                        {
                            await databaseHandler.ChangeUserName(inputName);
                        }
                        else
                        {
                            await DisplayAlert("Помилка", "Введено некоректне ім'я користувача, слійдуте інструкціям при додаванні.", "OK");
                        }
                    }
                    break;
                case "Змінити вік":
                    {
                        string inputAgeString = await DisplayPromptAsync("Налаштування профілю", "Введіть Ваш вік:", maxLength:2, keyboard:Keyboard.Numeric);
                        int.TryParse(inputAgeString, out int inputAge);
                        if(inputAge < 18)
                        {
                            await DisplayAlert("Попередження", "Застосування Індексу Маси Тіла для осіб до 18 років має проводитися з обережністю", "Зрозуміло!");
                            await databaseHandler.ChangeUserAge(inputAge);
                        }
                        else
                        {
                            await databaseHandler.ChangeUserAge(inputAge);
                        }
                    }
                    break;
                case "Вийти":
                    {
                        break;
                    }
            }
            await db.SaveChangesAsync();
        }
    }
}
