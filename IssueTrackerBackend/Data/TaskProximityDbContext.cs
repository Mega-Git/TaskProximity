﻿using IssueTrackerBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTrackerBackend.Data
{
    public class TaskProximityDbContext : DbContext
    {
        public TaskProximityDbContext(DbContextOptions<TaskProximityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // Add DbSet properties for your other entities/models

        // Additional DbSet properties for other entities can be added here
    }
}