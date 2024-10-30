using HealthApp.BindingHelpers;
using HealthApp.CoreLogic;
using HealthApp.Database;

namespace HealthApp
{
    public partial class ConsumingPage : ContentPage
    {
        public ConsumingPage()
        {
            InitializeComponent();

            OnAppearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //var medpr = new MedicinesProgressHandler();

            //medpr.MedProgressUpdate();

            var schHandler = new ScheduleHandler();

            schHandler.CreateTodaysSchedule();

            BindingContext = new ScheduleViewModel();
        }

        private async void OnShowListAlert(object sender, EventArgs e)
        {
            using (var db = new DatabaseSource())
            {
                var medicinesProgressList = db.medicines_progress.ToList();

                var displayText = string.Join(Environment.NewLine, medicinesProgressList.Select(mp =>
                    $"ID: {mp.id}, День: {mp.day_num}, Разів: {mp.times}"));

                await DisplayAlert("Прогрес медикаментів", displayText, "OK");
            }
            
        }

        private async void OnShowScheduleAlert(object sender, EventArgs e)
        {
            using (var db = new DatabaseSource())
            {
                var schedule = db.todays_schedule.ToList();

                var displayText = string.Join(Environment.NewLine,
                    schedule.Select(s => $"Медикамент: {s.MedName} - Час прийому: {s.reception_hour}"));

                await DisplayAlert("Розклад на сьогодні", displayText, "OK");
            }

        }

        private async void OnMedSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedsSettings());
        }
    }
}