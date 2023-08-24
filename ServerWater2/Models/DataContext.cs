using ServerWater2.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerWater2.Models
{
    public class DataContext : DbContext
    {
        public static Random random = new Random();
        public DbSet<SqlUser>? users { get; set; }
        public DbSet<SqlRole>? roles { get; set; }
        public DbSet<SqlLogOrder>? logs { get; set; }
        public DbSet<SqlOrder>? orders { get; set; }
        public DbSet<SqlCertificate>? certificates { get; set; }

        public DbSet<SqlGroup>? groups { get; set; }
        public DbSet<SqlArea>? areas { get; set; }
        public DbSet<SqlService>? services { get; set; }
        public DbSet<SqlType>? types { get; set; }
        public DbSet<SqlState>? states { get; set; }       
        //public DbSet<SqlArea>? areas { get; set; }
        public DbSet<SqlFile>? files { get; set; }
        public DbSet<SqlViewForm>? forms { get; set; }
        public DbSet<SqlCalcItems>? calcItems { get; set; }

        public DbSet<SqlCustomer>? customers { get; set; }
        public DbSet<SqlAction>? actions { get; set; }
        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string configSql = "Host=office.stvg.vn:59066;Database=db_stvg_ServerWater2;Username=postgres;Password=stvg";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(configSql);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<SqlOrder>().HasOne<SqlUser>(s => s.receiver).WithMany(s => s.receiverOrders);
            modelBuilder.Entity<SqlOrder>().HasOne<SqlUser>(s => s.manager).WithMany(s => s.managerOrders);
            modelBuilder.Entity<SqlOrder>().HasOne<SqlUser>(s => s.worker).WithMany(s => s.workerOrders);
            modelBuilder.Entity<SqlOrder>().HasOne<SqlUser>(s => s.survey).WithMany(s => s.surveyOrders);
        }
    }

}
