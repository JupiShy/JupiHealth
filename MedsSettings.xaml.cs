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
            _gridElements = new Dictionary<(int, int), View>();
        }

        private Dictionary<(int row, int column), View> _gridElements;

        private void AddRowMedTable()
        {
            
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

                //await _database.AddMed(new Medicines
                //{
                //    DrugName = medName,
                //    DaysToTake = Convert.ToInt16(medDays),
                //    ReceptionHours = medTime
                //});
            }



        }

        private async void OnChangeButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("PopUp", "Empty", "OK");
        }
    }
}