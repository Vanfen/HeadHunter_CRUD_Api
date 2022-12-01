using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using techTask_IB.Candidates;

namespace techTask_IB.DataModels
{
    public class Context: DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-MJFF56A\\SQLEXPRESS;Initial Catalog=TechnicalTaskDb;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique(true);
        }
    }
    public static class DbExtensions
    {
        public static T AddIfNotPresent<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            if (predicate == null)
                return null;
            if (!dbSet.Any(predicate))
            {
                dbSet.Add(entity);
                return entity;
            }
            return null;
        }
    }
}
