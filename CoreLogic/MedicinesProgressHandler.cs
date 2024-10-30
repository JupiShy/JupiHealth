using HealthApp.Database;
using HealthApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.CoreLogic
{
    class MedicinesProgressHandler
    {
        public async Task MedProgressUpdate()
        {
            await MedProgressAddNew();
        }

        public async Task MedProgressAddNew()
        {
            var dbHandler = new DatabaseHandler();

            var medicines = await dbHandler.GetMedicinesList();

            using (var db = new DatabaseSource())
            {
                foreach (var medicine in medicines)
                {
                    if (!await IsMedInMedicinesProgress(medicine.id))
                    {
                        var med_progress = new MedicinesProgress
                        {
                            id = medicine.id,
                            day_num = medicine.days_to_take,
                            times = ConvertTimeString(medicine.reception_hours)
                        };

                        db.medicines_progress.Add(med_progress);
                    }
                }
                await db.SaveChangesAsync();
            }     
        }

        public async Task<bool> IsMedInMedicinesProgress(int medId)
        {
            using (var db = new DatabaseSource())
            {
                return await db.medicines_progress.AnyAsync(mp => mp.med_id == medId);
            }
        }

        public int ConvertTimeString(string times)
        {
            if (string.IsNullOrWhiteSpace(times))
            {
                return 0;
            }

            var normalizedTimes = times.Split(',')
                                        .Select(t => t.Trim())
                                        .Where(t => !string.IsNullOrEmpty(t))
                                        .ToArray();

            return normalizedTimes.Length;
        }

        public List<string> TimeToList(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString))
            {
                return new List<string>();
            }

            var timeList = timeString.Split(',')
                                        .Select(t => t.Trim())
                                        .Where(t => !string.IsNullOrEmpty(t))
                                        .ToList();

            return timeList;
        }
    }
}
