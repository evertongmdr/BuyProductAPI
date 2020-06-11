using BuyProductAPI.Entites;
using Microsoft.EntityFrameworkCore;

namespace BuyProductAPI.Contexts
{
    public class BuyProductContext: DbContext
    {
        public BuyProductContext(DbContextOptions<BuyProductContext> options): base (options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Oders { get; set; }
        public DbSet<OrderProduct> OderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(o => o.Order)
                .WithMany(op => op.OrderProducts)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Product)
                .WithMany(op => op.OrderProducts)
                .HasForeignKey(p => p.ProductId);
           
            
            /*
            //Exemplo
            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasColumnType("decimal(9,2");
                */
        }

    }
}
