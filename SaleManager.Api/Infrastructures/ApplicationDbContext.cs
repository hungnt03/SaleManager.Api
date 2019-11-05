using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaleManager.Api.Entities;
using SaleManager.Api.Infrastructures.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleManager.Api.Infrastructures
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TransactionDetail>()
                .HasKey(c => new { c.Barcode, c.TracsactionId });
            base.OnModelCreating(builder);
        }

        public DbSet<Category> Category { set; get; }
        public DbSet<Customer> Customer { set; get; }
        public DbSet<Discount> Discount { set; get; }
        public DbSet<Product> Product { set; get; }
        public DbSet<Supplier> Supplier { set; get; }
        public DbSet<SysParam> SysParam { set; get; }
        public DbSet<Transaction> Transaction { set; get; }
        public DbSet<TransactionDetail> TransactionDetail { set; get; }
    }
}
