using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using BaseData.Model;

namespace BaseData.DataAccess
{
    public class MyDataContext : DbContext
    {
        private static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BaseDataCS"].ToString();

        public DbSet<User> Users { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<City> Citys { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<InRecord> InRecords { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<DeliveryMan> DeliveryMen { get; set; }

        public MyDataContext()
            : base(ConnectionString)
        {
            //延迟加载
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.AutoDetectChangesEnabled = true;  //自动监测变化，默认值为 true  
            this.Configuration.ProxyCreationEnabled = false;

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
