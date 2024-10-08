using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class Metrics
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public float Weight { get; set; }
        public float Height { get; set; }
        public float Bmi { get; set; }
        public float Water { get; set; }
    }
}
