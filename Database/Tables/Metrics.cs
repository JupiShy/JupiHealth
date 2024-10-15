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
        public double weight { get; set; }
        public double height { get; set; }
        public double bmi { get; set; }
        public double water { get; set; }
    }
}
