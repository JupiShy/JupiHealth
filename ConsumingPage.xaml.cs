namespace HealthApp
{
    public partial class ConsumingPage : ContentPage
    {
        public ConsumingPage()
        {
            InitializeComponent();
        }

        private async void OnMedSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MedsSettings());
        }
    }
}