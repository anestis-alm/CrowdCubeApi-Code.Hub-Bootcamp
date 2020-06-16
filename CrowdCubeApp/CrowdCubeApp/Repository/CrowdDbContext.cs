using CrowdCubeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Repository
{
    public class CrowdDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Backer> Backers { get; set; }
        public DbSet<ProjectCreator> ProjectCreators { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageFund> PackageFunds { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }


        //public readonly static string ConnectionString =
        //    "Server =localhost; " +
        //    "Database = CFApp; " +
        //    "User Id = sa; " +
        //    "Password = admin!@#123;";

        public readonly static string ConnectionString =
            "Server=tcp:crowdcubeappdbserver.database.windows.net,1433;Initial Catalog=CrowdCubeAppDb;Persist Security Info=False;User ID=serveradmin;Password=av29102002-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public CrowdDbContext(DbContextOptions<CrowdDbContext> options)
                : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Role");
        }


        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
