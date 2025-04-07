using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiProjectAnton
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "database.db");
        //        optionsBuilder.UseSqlite($"Filename={databasePath}");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>().HasData(
                new Contact { Id = 1, Name = "Vasili", Phone = "+37253013019", Email = "vasili@vasili.com" },
                new Contact { Id = 2, Name = "Mihail", Phone = "+3722323232", Email = "vasili@vasili.com" },
                new Contact { Id = 3, Name = "Sosed", Phone = "+3722323239", Email = "vasili@vasili.com" },
                new Contact { Id = 4, Name = "Lupa", Phone = "+37251516516", Email = "vasili@vasili.com" },
                new Contact { Id = 5, Name = "Pupa", Phone = "+3722323239", Email = "vasili@vasili.com" }
            );

            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Country>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Country>()
                .Property(c => c.Capital)
                .HasDefaultValue("No Capital");

            modelBuilder.Entity<Country>()
                .Property(c => c.Population)
                .HasDefaultValue(0);

            modelBuilder.Entity<Country>()
                .Property(c => c.Flag)
                .HasDefaultValue("https://via.placeholder.com/50x30");
        }
    }
}
