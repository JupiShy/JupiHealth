namespace HealthApp
{
	public partial class MedsSettings : ContentPage
	{
		public MedsSettings()
		{
			InitializeComponent();
            _gridElements = new Dictionary<(int, int), View>();
        }

        private Dictionary<(int row, int column), View> _gridElements;

        private void AddRowMedTable()
        {
            
        }

        // Анімація при натисканні
        private async void OnButtonPressed(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await button.ScaleTo(0.9, 50);
        }

        // Анімація при відпусканні
        private async void OnButtonReleased(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await button.ScaleTo(1, 50);
        }

        // Обробник кліку
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
                    await DisplayAlert("Помилка", "Неправильно введено назву, спробуйте ще раз.", "OK");
                }

                while (true)
                {
                    medDays = await DisplayPromptAsync("Днів", "Введіть, скільки днів Ви маєте приймати цей препарат:");
                    int days;
                    if (int.TryParse(medDays, out days) && days > 0)
                        break;
                    await DisplayAlert("Помилка", "Неправильно введено кількість днів, спробуйте ще раз.", "OK");
                }

                while (true)
                {
                    medTime = await DisplayPromptAsync("Години", "Вкажіть години прийому:");
                    if (!string.IsNullOrEmpty(medTime))
                        break;
                    await DisplayAlert("Помилка", "Неправильно введено час, спробуйте ще раз.", "OK");
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