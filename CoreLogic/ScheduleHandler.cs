using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.CoreLogic
{
    class ScheduleHandler
    {
        public async Task ScheduleFilling()
        {
            //var medHandler = new MedicinesProgressHandler();
            //await medHandler.MedProgressUpdate();

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

                    db.todays_schedule.RemoveRange(db.todays_schedule);
                    db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'todays_schedule'");
                    await db.SaveChangesAsync();
                    var med_progress = await db.medicines_progress.ToListAsync();

                    foreach (var med in med_progress)
                    {
                        var medicament = db.medicines.Find(med.id);
                        if (medicament.days_to_take > 0)
                        {
                            var medicines = await db.medicines.FindAsync(med.id);
                            var timeList = medHandler.TimeToList(medicines!.reception_hours);
                            var times = medHandler.ConvertTimeString(medicines.reception_hours);
                            for (int i = 0; i < times; i++)
                            {
                                var med_to_consume = new TodaysSchedule
                                {
                                    med_id = med.id,
                                    reception_hour = timeList[i]
                                };
                                db.todays_schedule.Add(med_to_consume);
                            }
                        }
                        else
                        {
                            var medicines = await db.medicines.FindAsync(med.id);
                            medicines.drug_name += " (Випито)";
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

                                if (!alreadyExists)
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
                        catch
                        {
                            Console.WriteLine(":(");
                        }
                
                }

                await db.SaveChangesAsync();
            }
        }

    }
}
