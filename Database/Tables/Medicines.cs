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

        [SQLite.Column("days")]
        public string DaysToTake { get; set; }

        [SQLite.Column("hours")]
        public int ReceptionHours { get; set; }
    }
}
