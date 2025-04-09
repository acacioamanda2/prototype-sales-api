using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<SaleItemEntity> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleEntity>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SaleItemEntity>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<SaleEntity>()
                .HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(i => i.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
