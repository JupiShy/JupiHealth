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
            double bmi = CalculateBMI(weight, height);

            if (bmi < 200)
            {
                metrics.bmi = bmi;
                await db.SaveChangesAsync();
            }   
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

        public async Task<User?> GetUserInfo()
        {
            return await db.user.FindAsync(1);
        }

        public async Task ChangeUserName(string name)
        {

            var user = await db.user.FindAsync(1);

            if (user != null)
            {
                user.name = name;

                db.user.Update(user);
            }
            else
            {
                user = new User
                {
                    id = 1,
                    name = name,
                    age = 0
                };

                db.user.Add(user);

            }

            await db.SaveChangesAsync();
        }

        public async Task ChangeUserAge(int age)
        {

            var user = await db.user.FindAsync(1);

            if (user != null)
            {
                user.age = age;

                db.user.Update(user);
            }
            else
            {
                user = new User
                {
                    id = 1,
                    name = "користувач",
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

            var lastMetrics = db.metrics
                                .OrderByDescending(m => m.date)
                                .Take(7)
                                .ToList();

            lastMetrics.Reverse();

            bool isNull = lastMetrics.All(m => m?.bmi == null);

            if (isNull)
            {
                for (int i = 0; i < 7; i++)
                {
                    LastMetricsList.Add(1);
                    DateMetricsList.Add("N/A");
                }
            }
            else
            {
                foreach (var metrics in lastMetrics)
                {
                    double metric = metrics?.bmi ?? 0;
                    string date = metrics?.date ?? "N/A";

                    LastMetricsList.Add(metric);
                    DateMetricsList.Add(date);
                }
            }

            for(int i = LastMetricsList.Count; i < 7; i++)
    {
                LastMetricsList.Add(0);
                DateMetricsList.Add("N/A");
            }

            var user = db.user.Find(1);

            var entries = new List<ChartEntry>();

            if (user != null)
            {
                entries.Add(new ChartEntry((float)user.target)
                {
                    Label = "Ціль",
                    ValueLabel = user.target.ToString(),
                    Color = SKColor.Parse("#49386b")
                });
            }

            for (int i = 0; i < LastMetricsList.Count; i++)
            {
                entries.Add(new ChartEntry((float)LastMetricsList[i])
                {
                    Label = DateMetricsList[i],
                    ValueLabel = LastMetricsList[i].ToString(),
                    Color = SKColor.Parse(GetColour(i))
                });
            }

            return entries;
        }

        private string GetColour(int index)
        {
            string[] colors = new string[]
            {
                "#624E88", "#745B9A", "#8967B3", "#A270A0", "#CB80AB", "#D39A96", "#E6D9A2"
            };

            return colors[index % colors.Length];
        }

        public async Task AddMedicine(string name, int days, string hours)
        {
            var meds = new Medicines
            {
                drug_name = name,
                days_to_take = days,
                reception_hours = hours
            };

            db.medicines.Add(meds);
            await db.SaveChangesAsync();
        }

        public async Task<List<Medicines>> GetMedicinesList()
        {
            var list = await db.medicines.ToListAsync();
            return list;
        }
    }
}
