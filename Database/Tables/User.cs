using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class User
    {
        public int id { get; set; }

        public string name { get; set; }

        public int age { get; set; }

        public double target { get; set; }
    }
}
