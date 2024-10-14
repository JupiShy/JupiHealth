using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class TodaysSchedule
    {
        public int id { get; set; }
        public int med_id { get; set; }
        public string reception_hour { get; set; }
    }
}
