using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database.Tables
{
    public class MedicinesProgress
    {
        public int id {  get; set; }

        public int med_id { get; set; }

        public int day_num { get; set; }

        public int times {  get; set; }
    }
}
