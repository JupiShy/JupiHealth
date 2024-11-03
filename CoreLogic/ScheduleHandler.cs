using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.CoreLogic
{
    class ScheduleHandler
    {
        public async Task ScheduleFilling()
        {
            using (var db = new DatabaseSource())
            {
                var medpr = db.medicines_progress.FirstOrDefault();

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd");

                Console.WriteLine("A " + medpr.last_schedule_date + " B " + todayDate);

                if (medpr.last_schedule_date != todayDate)
                {
                    Console.WriteLine("CREATE!");
                    await CreateTodaysSchedule();
                }
                else
                {
                    Console.WriteLine("UPDATE!");
                    await UpdateTodaysSchedule();
                }
            }
        }

        public async Task CreateTodaysSchedule()
        {
            using (var db = new DatabaseSource())
            {
                var dbHandler = new DatabaseHandler();
                var medHandler = new MedicinesProgressHandler();

                var medpr = db.medicines_progress.FirstOrDefault();

                db.Database.ExecuteSqlRaw("DELETE FROM todays_schedule");
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'todays_schedule'");
                    await db.SaveChangesAsync();
                    var med_progress = await db.medicines_progress.ToListAsync();

                    foreach (var med in med_progress)
                    {
                    Console.WriteLine("Medicine processing...");
                    var medicament = db.medicines.Find(med.id);
                    Console.WriteLine($"Processing medicine ID: {med.id} with days_to_take: {med.day_num}");

                    if (med?.day_num >= 1)
                        {
                        Console.WriteLine("Days > 0");
                            var medicines = await db.medicines.FindAsync(med.med_id);
                            var timeList = medHandler.TimeToList(medicines!.reception_hours);
                            var times = medHandler.ConvertTimeString(medicines.reception_hours);
                            for (int i = 0; i < times; i++)
                            {
                                var med_to_consume = new TodaysSchedule
                                {
                                    med_id = med.med_id,
                                    reception_hour = timeList[i]
                                };
                                db.todays_schedule.Add(med_to_consume);
                            }

                        med.last_schedule_date = DateTime.Today.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                        Console.WriteLine("Days < 0");
                        }
                    }
                    await db.SaveChangesAsync();
            }
        }

        public async Task UpdateTodaysSchedule()
        {
            using (var db = new DatabaseSource())
            {
                var dbHandler = new DatabaseHandler();
                var medHandler = new MedicinesProgressHandler();

                var medProgressList = await db.medicines_progress.ToListAsync();

                foreach (var medProgress in medProgressList)
                {
                        var medicine = await db.medicines.FindAsync(medProgress.med_id);

                        try
                        {
                            var timeList = medHandler.TimeToList(medicine.reception_hours);
                            var times = medHandler.ConvertTimeString(medicine.reception_hours);

                            for (int i = 0; i < times; i++)
                            {
                                bool alreadyExists = await db.todays_schedule.AnyAsync(ts =>
                                    ts.med_id == medicine.id && ts.reception_hour == timeList[i]);

                                if (!alreadyExists && medProgress.day_num > 0)
                                {
                                    Console.WriteLine("ADDED!");

                                    var med_to_consume = new TodaysSchedule
                                    {
                                        med_id = medicine.id,
                                        reception_hour = timeList[i]
                                    };
                                    db.todays_schedule.Add(med_to_consume);
                                }
                                else
                                {
                                    Console.WriteLine("NOT ADDED, ALREADY EXIST!");
                                }
                            }
                        }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка при оновленні графіку для медикаменту {medicine?.drug_name ?? "невідомий"}: {ex.Message}");
                    }
                }
                await db.SaveChangesAsync();
            }
        }

    }
}
