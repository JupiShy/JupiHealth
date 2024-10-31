namespace HealthApp.Database.Tables
{
    public class TodaysSchedule
    {
        public int id { get; set; }
        public int med_id { get; set; }
        public string? reception_hour { get; set; }
        public int is_consumed { get; set; }

        public string MedName
        {
            get
            {
                using (var db = new DatabaseSource())
                {
                    var medicine = db.medicines.Find(med_id);
                    return medicine != null ? medicine.drug_name : "Невідомий медикамент";
                }
            }
        }
    }
}
