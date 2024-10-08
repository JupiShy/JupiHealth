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

                db.SaveChanges();     
            }
        }   
    }
}
