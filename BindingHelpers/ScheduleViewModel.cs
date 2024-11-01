using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;


namespace HealthApp.BindingHelpers
{
    class ScheduleViewModel
    {
        public ObservableCollection<TodaysSchedule> obs_schedule { get; set; } = new ObservableCollection<TodaysSchedule>();

        private DatabaseSource _db;

        public ScheduleViewModel()
        {
            _db = new DatabaseSource();
            _db.Database.EnsureCreated();
            LoadMed();
        }


        private async Task LoadMed()
        {
            var schedule = await _db.todays_schedule.ToListAsync();
            obs_schedule.Clear();

            foreach (var row in schedule)
            {
                if(row.is_consumed != 1) obs_schedule.Add(row);
            }
        }

        public async Task MarkAsConsumed(TodaysSchedule schedule)
        {
            using (var db = new DatabaseSource())
            {
                var itemToUpdate = await db.todays_schedule.FindAsync(schedule.id);
                if (itemToUpdate != null)
                {
                    itemToUpdate.is_consumed = 1;
                    await db.SaveChangesAsync();
                    obs_schedule.Remove(schedule);
                }

                bool allConsumed = !db.todays_schedule
                    .Any(ts => ts.med_id == schedule.med_id && ts.is_consumed == 0);

                if (allConsumed)
                {
                    var medicineProgress = await db.medicines_progress.FirstOrDefaultAsync(mp => mp.med_id == schedule.med_id);
                    if (medicineProgress != null && medicineProgress.day_num > 0)
                    {
                        medicineProgress.day_num -= 1;
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
