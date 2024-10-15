using HealthApp.BindingHelpers;
using HealthApp.Database;
using Microcharts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SkiaSharp;

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
        }

        public async void ChangeParametersButtonClicked(object sender, EventArgs e)
        {
            string weightStr = await DisplayPromptAsync("Оновити параметри", "Введіть Вашу вагу (кг):", "OK", keyboard: Keyboard.Numeric);
            await Task.Delay(800);
            string heightStr = await DisplayPromptAsync("Оновити параметри", "Введіть Ваш зріст (см):", "OK", keyboard: Keyboard.Numeric);

            if (double.TryParse(weightStr, out double weight) && double.TryParse(heightStr, out double height))
            {
                using (var db = new DatabaseSource())
                {
                    var dbHandler = new DatabaseHandler();

                    await dbHandler.ChangeParameters(weight, height);

                    await db.SaveChangesAsync();
                }

                OnAppearing();
            }
        }

        public async void ChangeTargetButtonClicked(object sender, EventArgs e)
        {
            string targetStr = await DisplayPromptAsync("Змінити цільовий ІМТ", "Введіть Ваш цільовий ІМТ. " + Environment.NewLine +
                "Майте на увазі, що здоровий показник від 18,5 до 24,9!", "OK", keyboard: Keyboard.Numeric);

            if (double.TryParse(targetStr, out double target))
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
            string input = await DisplayPromptAsync("Додати воду", "Введіть кількість (мл):", "OK", keyboard: Keyboard.Numeric);

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

        private void CreateBarChart()
        {
            var entries = new List<ChartEntry>
            {
                new ChartEntry(NormalizeValue((float)24.0))
                {
                    Label = "ПН",
                    ValueLabel = "25.0",
                    Color = SKColor.Parse("#624E88")
                },
                new ChartEntry(NormalizeValue((float)24.7))
                {
                    Label = "ВТ",
                    ValueLabel = "25.1",
                    Color = SKColor.Parse("#745B9A")
                },
                new ChartEntry(NormalizeValue((float) 25.8))
                {
                    Label = "СР",
                    ValueLabel = "24.8",
                    Color = SKColor.Parse("#8967B3")
                },
                new ChartEntry(NormalizeValue((float) 26.1))
                {
                    Label = "ЧТ",
                    ValueLabel = "24.5",
                    Color = SKColor.Parse("#A270A0")
                },
                new ChartEntry(NormalizeValue((float) 25.3))
                {
                    Label = "ПТ",
                    ValueLabel = "24.3",
                    Color = SKColor.Parse("#CB80AB")
                },
                new ChartEntry(NormalizeValue((float) 24.0))
                {
                    Label = "СБ",
                    ValueLabel = "23.0",
                    Color = SKColor.Parse("#D39A96")
                },
                new ChartEntry(NormalizeValue((float) 23.4))
                {
                    Label = "НД",
                    ValueLabel = "23.4",
                    Color = SKColor.Parse("#E6D9A2")
                }
            };

            ChartWeek.Chart = new BarChart {
                Entries = entries
            }; 

        }
        public float NormalizeValue(float value)
        {
            float min = 10;
            float max = 50;
            return (value - min) / (max - min) * 100;
        }
    }
}
