using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async void LoadMed()
        {
            var schedule = await _db.todays_schedule.ToListAsync();
            obs_schedule.Clear();

            foreach (var row in schedule)
            {
                obs_schedule.Add(row);
            }
        }
    }
}
