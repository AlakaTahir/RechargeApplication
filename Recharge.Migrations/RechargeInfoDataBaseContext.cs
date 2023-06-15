using Microsoft.EntityFrameworkCore;
using Recharge.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;


namespace Recharge.Migrations
{
    public class RechargeInfoDataBaseContext:DbContext
    {
        public RechargeInfoDataBaseContext(DbContextOptions<RechargeInfoDataBaseContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }
        public DbSet<TransactionHistory>TransactionHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable("UserInfos");
            modelBuilder.Entity<UserBalance>().ToTable("UserBalances");
            modelBuilder.Entity<TransactionHistory>().ToTable("TransactionHistories");
        }

    }
}
