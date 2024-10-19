using HealthApp.Database.Tables;
using Microcharts;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace HealthApp.Database
{
    public class DatabaseHandler
    {
        public readonly DatabaseSource db = new DatabaseSource();

        public async Task ChangeParameters(double weight, double height)
        {
            await AddMetricsIfNotExistsAsync();

            var metrics = await db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefaultAsync();

            metrics.weight = weight;
            metrics.height = height;
            metrics.bmi = CalculateBMI(weight, height);

            await db.SaveChangesAsync();
        }

        public async Task AddWater(int amount)
        {
            await AddMetricsIfNotExistsAsync();

            var metrics = await db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefaultAsync();

            var result = metrics.water += amount;

            if (result <= 8000)
            {
                metrics.water = result;
                await db.SaveChangesAsync();
            }
            else
            {
                metrics.water = 8000;
                await db.SaveChangesAsync();
            }
        }

        public async Task ChangeTarget(double target)
        {
            var user = await db.user.FindAsync(1);

            if (user != null)
            {
                user.target = target;

                db.user.Update(user);
            }
            else
            {
                user = new User
                {
                    id = 1,
                    name = "користувач",
                    age = 0,
                    target = target
                };

                db.user.Add(user);
            }


            await db.SaveChangesAsync();
        }

        public async Task ChangeUserInfo(string name, int age)
        {

            var user = await db.user.FindAsync(1);

            if (user != null)
            {
                user.name = name;
                user.age = age;

                db.user.Update(user);
            }
            else
            {
                user = new User
                {
                    id = 1,
                    name = name,
                    age = age
                };

                db.user.Add(user);

            }

            await db.SaveChangesAsync();
        }

        public static async Task AddMetricsIfNotExistsAsync()
        {
            using (var db = new DatabaseSource())
            {
                var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                var checkExist = await db.metrics
                    .FirstOrDefaultAsync(m => m.date == currentDate);

                if (checkExist == null)
                {
                    var newMetric = new Metrics
                    {
                        date = currentDate,
                    };

                    await db.metrics.AddAsync(newMetric);

                    await db.SaveChangesAsync();
                }
            }
        }

        public double CalculateBMI(double weight, double height)
        {
            return Math.Round(weight /Math.Pow(height/100, 2), 1);
        }

        public List<ChartEntry> CalculateChart()
        {
            var LastMetricsList = new List<double>();
            var DateMetricsList = new List<string>();

            for (int i = 1; i <= 7; i++)
            {
                var metrics = db.metrics.Find(i);
                double metric;
                string date;

                if (metrics == null)
                {
                    metric = 0;
                    date = "N/A";
                }
                else
                {
                    metric = metrics.bmi;
                    date = metrics.date;
                }

                LastMetricsList.Add(metric);
                DateMetricsList.Add(date);
            }

            var entries = new List<ChartEntry>
    {
        new ChartEntry((float)LastMetricsList[0]) // Індекси починаються з 0
        {
            Label = DateMetricsList[0],
            ValueLabel = LastMetricsList[0].ToString(),
            Color = SKColor.Parse("#624E88")
        },
        new ChartEntry((float)LastMetricsList[1])
        {
            Label = DateMetricsList[1],
            ValueLabel = LastMetricsList[1].ToString(),
            Color = SKColor.Parse("#745B9A")
        },
        new ChartEntry((float)LastMetricsList[2])
        {
            Label = DateMetricsList[2],
            ValueLabel = LastMetricsList[2].ToString(),
            Color = SKColor.Parse("#8967B3")
        },
        new ChartEntry((float)LastMetricsList[3])
        {
            Label = DateMetricsList[3],
            ValueLabel = LastMetricsList[3].ToString(),
            Color = SKColor.Parse("#A270A0")
        },
        new ChartEntry((float)LastMetricsList[4])
        {
            Label = DateMetricsList[4],
            ValueLabel = LastMetricsList[4].ToString(),
            Color = SKColor.Parse("#CB80AB")
        },
        new ChartEntry((float)LastMetricsList[5])
        {
            Label = DateMetricsList[5],
            ValueLabel = LastMetricsList[5].ToString(),
            Color = SKColor.Parse("#D39A96")
        },
        new ChartEntry((float)LastMetricsList[6])
        {
            Label = DateMetricsList[6],
            ValueLabel = LastMetricsList[6].ToString(),
            Color = SKColor.Parse("#E6D9A2")
        }
    };

            return entries;
        }


        private float NormalizeValue(float value)
        {
            float min = 10;
            float max = 50;
            return (value - min) / (max - min) * 100;
        }
    }
}
