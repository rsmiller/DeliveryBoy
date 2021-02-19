using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentCreatorSearch.Business.Configuration;
using DeliveryBoy.BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBoy.BusinessLayer
{
    public interface ITipsContext
    {
        DbSet<Tip> Tips { get; set; }
        DbSet<TipDetail> TipDetails { get; set; }
        DbSet<UserTip> UserTips { get; set; }
        int SaveChanges();
    }
    public class TipsContext : DbContext, ITipsContext
    {
        internal const string AppSchemaName = "";

        public IDatabaseConnectionSettings ConnectionSettings { get; set; }

        public TipsContext(IDatabaseConnectionSettings configuration)
        {
            this.ConnectionSettings = configuration;
        }

        public TipsContext(DbContextOptions<TipsContext> options, IDatabaseConnectionSettings configuration)
            : base(options)
        {
            this.ConnectionSettings = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseMySQL(this.ConnectionSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TipsConfiguration());
            modelBuilder.ApplyConfiguration(new TipDetailConfiguration());
            modelBuilder.ApplyConfiguration(new UserTipsConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Tip> Tips { get; set; }
        public DbSet<TipDetail> TipDetails { get; set; }
        public DbSet<UserTip> UserTips { get; set; }
    }
}
