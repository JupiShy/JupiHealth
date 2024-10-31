using HealthApp.BindingHelpers;
using HealthApp.CoreLogic;
using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp
{
    public partial class ConsumingPage : ContentPage
    {
        public ConsumingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
            var medHandler = new MedicinesProgressHandler();
            medHandler.MedProgressUpdate();

            var schHandler = new ScheduleHandler();
            schHandler.ScheduleFilling();

            BindingContext = new ScheduleViewModel();
        }

        private async void OnShowListAlert(object sender, EventArgs e)
        {
            using (var db = new DatabaseSource())
            {
                var medicinesProgressList = db.medicines_progress.ToList();

                var displayText = string.Join(Environment.NewLine, medicinesProgressList.Select(mp =>
                    $"ID: {mp.id}, MedID: {mp.med_id}, День: {mp.day_num}, Разів: {mp.times}, Останній день: {mp.last_schedule_date}"));

                await DisplayAlert("Прогрес медикаментів", displayText, "OK");
            }
            
        }

        private async void OnShowScheduleAlert(object sender, EventArgs e)
        {
            using (var db = new DatabaseSource())
            {
                var schedule = db.todays_schedule.ToList();
                var displayText = string.Join(Environment.NewLine,
                    schedule.Select(s => $"Медикамент: {s.MedName} - Час прийому: {s.reception_hour} - Випито: {s.is_consumed}"));
                await DisplayAlert("Розклад на сьогодні", displayText, "OK");
            }
        }


        private async void OnScheduleTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selectedItem = e.Item as TodaysSchedule;
            if (selectedItem == null)
                return;

            var viewModel = (ScheduleViewModel)BindingContext;

            bool isDelete = await DisplayAlert("Медикамент випито", "Відмітити медикамент, як прийнятий? Цю дію не можна відмінити", "Так", "Ні");
            if (isDelete)
            {
                try
                {
                    await viewModel.MarkAsConsumed(selectedItem);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Помилка", "Не вдалося видалити медикамент: " + ex.Message, "OK");
                }
            }

            ((ListView)sender).SelectedItem = null;
        }

        


        private async void OnMedSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedsSettings());
        }
    }
}