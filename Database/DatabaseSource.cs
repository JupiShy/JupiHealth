﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthApp.Database.Tables;

namespace HealthApp.Database
{
    public class DatabaseSource : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Metrics> metrics { get; set; }
        public DbSet<Medicines> medicines { get; set; }
        public DbSet<MedicinesProgress> medicines_progress { get; set; }
        public DbSet<TodaysSchedule> todays_schedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UserData.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        public void InitializeDatabase()
        {
            try
            {
                this.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }
    }
}
