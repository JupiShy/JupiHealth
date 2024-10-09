using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class Metrics
    {
        public int id { get; set; }
        public string date { get; set; }
        public float weight { get; set; }
        public float height { get; set; }
        public float bmi { get; set; }
        public float water { get; set; }
    }
}
