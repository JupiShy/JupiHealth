using HealthApp.BindingHelpers;
using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp
{
    public partial class MedsSettings : ContentPage
    {
        private int _medId;

        public MedsSettings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = new MedViewModel();
        }

        private async void OnAddMedButtonClicked(object sender, EventArgs e)
        {
            string medName = "null";
            string medDays = "null";
            string medTime = "null";
            int days = 0;
            bool answer = await DisplayAlert("Редагування", "Додати новий медикамент?", "OK", "Назад");

            if (answer)
            {
                while (true)
                {
                    medName = await DisplayPromptAsync("Назва", "Введіть назву (до 15 символів):", maxLength:15);
                    if (!string.IsNullOrEmpty(medName))
                        break;
                    bool ans = await DisplayAlert("Помилка", "Неправильно введено назву, спробуйте ще раз.", "OK", "Назад");
                    if (ans == false) break;
                }

                while (true)
                {
                    medDays = await DisplayPromptAsync("Днів", "Введіть, скільки днів Ви маєте приймати цей препарат:", maxLength:3, keyboard:Keyboard.Numeric);
                    if (int.TryParse(medDays, out days) && days > 0)
                        break;
                    bool ans = await DisplayAlert("Помилка", "Неправильно введено кількість днів, спробуйте ще раз.", "OK", "Назад");
                    if (ans == false) break;
                }

                while (true)
                {
                    medTime = await DisplayPromptAsync("Години", "Вкажіть години прийому (через кому):", maxLength: 40);
                    if (!string.IsNullOrEmpty(medTime))
                    {
                        using (var db = new DatabaseSource())
                        {
                            var dbHandler = new DatabaseHandler();

                            await dbHandler.AddMedicine(medName, days, medTime);
                        }

                        await DisplayAlert("Медикамент додано", "Ви можете редагувати та видаляти медикаменти, " +
                            "натиснувши на потрібний медикамент", "OK");
                        break;
                    }
                    else 
                    {
                        bool ans = await DisplayAlert("Помилка", "Неправильно введено час, спробуйте ще раз.", "OK", "Назад");
                        if (ans == false) break;
                    }
                }   
            }

            OnAppearing();
        }


        private async void OnMedicineTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var selectedMedicine = e.Item as Medicines;
            var action = await DisplayActionSheet($"Виберіть дію для {selectedMedicine.drug_name}", "Вийти", null, "Редагувати", "Видалити");
            var viewModel = (MedViewModel)BindingContext;

            switch (action)
            {
                case "Редагувати":
                    string newName = await DisplayPromptAsync("Редагувати", "Назва:", initialValue: selectedMedicine.drug_name, maxLength:15);
                    string newDaysStr = await DisplayPromptAsync("Редагувати", "Дні прийому:", initialValue: selectedMedicine.days_to_take.ToString(), maxLength:3, keyboard:Keyboard.Numeric);
                    string newHours = await DisplayPromptAsync("Редагувати", "Час прийому:", initialValue: selectedMedicine.reception_hours, maxLength:40);

                    if (int.TryParse(newDaysStr, out int newDays) && !string.IsNullOrWhiteSpace(newName) && !string.IsNullOrWhiteSpace(newHours))
                    {
                        await viewModel.EditMed(selectedMedicine, newName, newDays, newHours);
                    }
                    else
                    {
                        await DisplayAlert("Помилка", "Некоректні дані", "OK");
                    }
                    break;

                case "Видалити":
                    bool isDelete = await DisplayAlert("Підтвердження", "Ви впевнені, що хочете видалити цей медикамент?", "Так", "Ні");
                    if (isDelete)
                    {
                        await viewModel.DeleteMed(selectedMedicine);
                    }
                    break;

                case "Вийти":
                    break;
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}