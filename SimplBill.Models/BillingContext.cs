using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplBill.Models
{
    public class BillingContext : DbContext
    {
        public BillingContext()
        {
            Database.Migrate();
        }

        public DbSet<BillDetailModel> BillDetails { get; set; }

        public DbSet<CustomerModel> CustomerDetails { get; set; }

        public DbSet<ProductInfoModel> ProductInfoDetails { get; set; }

        public DbSet<ProductDetailModel> ProductDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BillingDB;Trusted_Connection=True;");
        }

    }
}
