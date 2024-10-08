using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class MedicationSchedule
    {
        public int Id { get; set; }
        public int MedId { get; set; }
        public int MedNumber { get; set; }
        public int Day { get; set; }
        public string ReceptionHour { get; set; }
    }
}
