using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database
{
    public class DatabaseHandler
    {
        public readonly DatabaseSource db = new DatabaseSource();

        public async Task AddWater(int amount)
        {
            await AddMetricsIfNotExistsAsync();

            var metrics = await db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefaultAsync();

            metrics.water += amount;

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

    }
}
