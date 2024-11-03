using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using HealthApp.Database;
using HealthApp.Database.Tables;
using HealthApp.CoreLogic;

namespace HealthApp.BindingHelpers
{
    public class MedViewModel
    {
        public ObservableCollection<Medicines> medicines { get; set; } = new ObservableCollection<Medicines>();

        private DatabaseSource _db;

        public MedViewModel()
        {
            _db = new DatabaseSource();
            _db.Database.EnsureCreated();
            LoadMed();
        }

        private async void LoadMed()
        {
            var medicinesList = await _db.medicines.ToListAsync();
            medicines.Clear();

            foreach (var medicine in medicinesList)
            {
                medicines.Add(medicine);
            }
        }

        public async Task EditMed(Medicines medicine, string newName, int newDays, string newHours)
        {
            var old_days_value = medicine.days_to_take;
            var old_hours_value = medicine.reception_hours;
            
            medicine.drug_name = newName;
            medicine.days_to_take = newDays;
            medicine.reception_hours = newHours;

            var medProgress = await _db.medicines_progress.FirstOrDefaultAsync(mp => mp.med_id == medicine.id);

            if (medProgress != null)
            {
                if(newDays != old_days_value)
                {
                   medProgress.day_num = newDays;
                }

                if(newHours != old_hours_value)
                {
                    var medHandler = new MedicinesProgressHandler();
                    medProgress.times = medHandler.ConvertTimeString(medicine.reception_hours);
                }
            }

            _db.medicines.Update(medicine);
            await _db.SaveChangesAsync();
            LoadMed();
        }

        public async Task DeleteMed(Medicines medicine)
        {
            _db.medicines.Remove(medicine);
            var medProgress = await _db.medicines_progress.FirstOrDefaultAsync(mp => mp.med_id == medicine.id);

            if (medProgress != null)
            {
                _db.medicines_progress.Remove(medProgress);
            }

            await _db.SaveChangesAsync();
            medicines.Remove(medicine);
        }
    }
}
