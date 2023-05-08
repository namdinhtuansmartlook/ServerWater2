using Microsoft.EntityFrameworkCore;

namespace ServerWater2.Models
{
    public class DataContext : DbContext
    {
        public static Random random = new Random();
       
        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //public static string configSql = "Host=office.stvg.vn:59066;Database=db_stvg_GIS;Username=postgres;Password=stvg";

        public static string configSql = "";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(configSql);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<SqlArea>().HasMany<SqlUser>(s => s.users).WithMany(s => s.areas);
            //modelBuilder.Entity<SqlDevice>().HasOne<SqlArea>(s => s.area).WithMany(s => s.devices);
        }
    }

}
