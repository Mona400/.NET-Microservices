﻿using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System.Reflection;

namespace PlatformService.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }
        public DbSet<Platform>Platforms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Identity column configuration.
        }


    }
}
