using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.CoreLogic
{
    class ScheduleHandler
    {
        public async Task CreateTodaysSchedule()
        {
            using (var db = new DatabaseSource())
            {
                var dbHandler = new DatabaseHandler();
                var medHandler = new MedicinesProgressHandler();

                db.todays_schedule.RemoveRange(db.todays_schedule);
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'todays_schedule'");
                await db.SaveChangesAsync();

                var med_progress = await db.medicines_progress.ToListAsync();
                

                foreach(var med in med_progress)
                {
                    var medicines = await db.medicines.FindAsync(med.id);

                    var timeList = medHandler.TimeToList(medicines!.reception_hours);
                    var times = medHandler.ConvertTimeString(medicines.reception_hours);

                    for (int i = 0; i < times; i++) {
                        var med_to_consume = new TodaysSchedule
                        {
                            med_id = med.id,
                            reception_hour = timeList[i]
                        };

                        db.todays_schedule.Add(med_to_consume);
                    }   
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
