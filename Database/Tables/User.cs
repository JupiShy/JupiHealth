using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    [Table("user")]
    public class User
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }
    }
}
