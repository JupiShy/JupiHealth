using Microcharts;
using SkiaSharp;

namespace HealthApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CreateBarChart();
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
