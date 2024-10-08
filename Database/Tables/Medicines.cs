using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    [SQLite.Table("medicines")]
    public class Medicines
    {
        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }

        [SQLite.Column("drug_name")]
        public string DrugName { get; set; }

        [SQLite.Column("days_to_take")]
        public int DaysToTake { get; set; }

        [SQLite.Column("reception_hours")]
        public string ReceptionHours { get; set; }
    }
}
