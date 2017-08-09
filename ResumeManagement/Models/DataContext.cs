namespace ResumeManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<CVLetter> CVLetters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<IdentityUserLogin>();
            modelBuilder.Ignore<IdentityUserRole>();
            modelBuilder.Ignore<IdentityUserClaim>();

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            modelBuilder.Entity<CVLetter>()
                .HasMany(e => e.Resumes)
                .WithRequired(e => e.CVLetter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resume>()
                .HasMany(e => e.CVLetters)
                .WithRequired(e => e.Resume)
                .WillCascadeOnDelete(false);
        }
    }
}
