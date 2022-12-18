using Microsoft.EntityFrameworkCore;

namespace MyWebShowStep.Data
{
    public class StepShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<Gpu> Gpus { get; set; }

        public DbSet<Cpu> Cpus { get; set; }

        public DbSet<Monitor> Monitors { get; set; }

        public StepShopContext()
        {

        }

        public StepShopContext(DbContextOptions<StepShopContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-R22J42Q;Initial Catalog=StepShop;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Trusted_Connection=True", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
