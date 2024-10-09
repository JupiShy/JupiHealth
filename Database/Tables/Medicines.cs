using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class Medicines
    {
        public int id { get; set; }

        public string drug_name { get; set; }

        public int days_to_take { get; set; }

        public string reception_hours { get; set; }
    }
}
