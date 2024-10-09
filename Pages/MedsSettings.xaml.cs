using HealthApp.Database;
using HealthApp.Database.Tables;

namespace HealthApp
{
	public partial class MedsSettings : ContentPage
	{
        private int _medId;

        public MedsSettings()
		{
			InitializeComponent();
        }
    
        private async void OnAddMedButtonClicked(object sender, EventArgs e)
        {
            string medName;
            string medDays;
            string medTime;
            bool answer = await DisplayAlert("Редагування", "Додати новий медикамент?", "OK", "Назад");

            if (answer)
            {
                while (true)
                {
                    medName = await DisplayPromptAsync("Назва", "Введіть назву:");
                    if (!string.IsNullOrEmpty(medName))
                        break;
                    await DisplayAlert("Помилка", "Неправильно введено назву, спробуйте ще раз.", "OK", "Назад");
                }

                while (true)
                {
                    medDays = await DisplayPromptAsync("Днів", "Введіть, скільки днів Ви маєте приймати цей препарат:");
                    int days;
                    if (int.TryParse(medDays, out days) && days > 0)
                        break;
                    await DisplayAlert("Помилка", "Неправильно введено кількість днів, спробуйте ще раз.", "OK", "Назад");
                }

                while (true)
                {
                    medTime = await DisplayPromptAsync("Години", "Вкажіть години прийому:");
                    if (!string.IsNullOrEmpty(medTime))
                        break;
                    await DisplayAlert("Помилка", "Неправильно введено час, спробуйте ще раз.", "OK", "Назад");
                }

                await DisplayAlert("Добре!", "Все чудово", "OK");
            }
        }

        private async void OnChangeButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("PopUp", "Empty", "OK");
        }
    }
}