using HealthApp.BindingHelpers;
using HealthApp.Database;
using Microcharts;

namespace HealthApp
{
    public partial class MainPage : ContentPage
    {
        private ViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            CreateBarChart();

            viewModel = new ViewModel();
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.UpdateData();

            CreateBarChart();
        }

        public async void ChangeParametersButtonClicked(object sender, EventArgs e)
        {
            string weightStr = await DisplayPromptAsync("Оновити параметри", "Введіть Вашу вагу (кг):", "OK", keyboard: Keyboard.Numeric, maxLength:5);
            double.TryParse(weightStr, out double weight);
            if(weight > 0)
            {
                await Task.Delay(800);
                string heightStr = await DisplayPromptAsync("Оновити параметри", "Введіть Ваш зріст (см):", "OK", keyboard: Keyboard.Numeric, maxLength: 5);
                double.TryParse(heightStr, out double height);

                if (height > 0)
                {
                    using (var db = new DatabaseSource())
                    {
                        var dbHandler = new DatabaseHandler();

                        await dbHandler.ChangeParameters(weight, height);

                        //await db.SaveChangesAsync();
                    }

                    OnAppearing();
                }
            }
            

            
        }

        public async void ChangeTargetButtonClicked(object sender, EventArgs e)
        {
            string targetStr = await DisplayPromptAsync("Змінити цільовий ІМТ", "Введіть Ваш цільовий ІМТ. " + Environment.NewLine +
                "Майте на увазі, що здоровий показник від 18,5 до 24,9!", "OK", keyboard: Keyboard.Numeric, maxLength: 5);

            if (double.TryParse(targetStr, out double target) && target > 15 && target < 200)
            {
                using (var db = new DatabaseSource())
                {
                    var dbHandler = new DatabaseHandler();

                    await dbHandler.ChangeTarget(target);

                    await db.SaveChangesAsync();
                }

                OnAppearing();
            }
        }


        public async void AddWaterButtonClicked(object sender, EventArgs e)
        {
            string input = await DisplayPromptAsync("Додати воду", "Введіть кількість (мл):", "OK", keyboard: Keyboard.Numeric, maxLength: 4);

            if (int.TryParse(input, out int amount))
            {
                using (var db = new DatabaseSource())
                {
                    var dbHandler = new DatabaseHandler();

                    await dbHandler.AddWater(amount);

                    await db.SaveChangesAsync();
                }

                OnAppearing();
            }

        }

        public int GetLabelTextSize()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                return 36;
            }
            else
            {
                return 16;
            }
        }

        private void CreateBarChart()
        {
            using (var db = new DatabaseSource())
            {
                var dbHandler = new DatabaseHandler();
                var entries = dbHandler.CalculateChart();

                LastDaysChart.Chart = new BarChart
                {
                    Entries = entries,
                    Margin = 22,
                    LabelTextSize = GetLabelTextSize(),
                    ValueLabelTextSize = GetLabelTextSize(),
                    ValueLabelOrientation = Orientation.Horizontal
                };
            }
        }
    }
}
