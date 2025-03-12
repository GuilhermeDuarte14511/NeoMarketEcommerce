using Microsoft.EntityFrameworkCore;
using NeoMarket.Domain.Entities;

namespace NeoMarket.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<StoreCarouselImage> StoreCarouselImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CustomerStore> CustomerStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureStore(modelBuilder);
            ConfigureCustomerStore(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigureCart(modelBuilder);
            ConfigureOrder(modelBuilder);
            ConfigurePayment(modelBuilder);
            ConfigureStoreCarouselImage(modelBuilder);
            ConfigureAddress(modelBuilder);
            ConfigureCategory(modelBuilder);
            ConfigureSubcategory(modelBuilder);
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.CPF)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.RG)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.RG)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.CPF)
                .HasMaxLength(14);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            // Relacionamento 1:N entre Store e Users (Usuários da loja)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.StoreId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureStore(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasKey(s => s.StoreId);

            modelBuilder.Entity<Store>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Store>()
                .Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Store>()
                .Property(s => s.UrlSlug)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Store>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Store>()
                .Property(s => s.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.CarouselImages)
                .WithOne(c => c.Store)
                .HasForeignKey(c => c.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Store)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento 1:1 entre Store e o Administrador (User)
            modelBuilder.Entity<Store>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Store>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureCustomerStore(ModelBuilder modelBuilder)
        {
            // Tabela intermediária para vincular clientes a lojas
            modelBuilder.Entity<CustomerStore>()
                .HasKey(cs => new { cs.UserId, cs.StoreId });

            modelBuilder.Entity<CustomerStore>()
                .HasOne(cs => cs.User)
                .WithMany()
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerStore>()
                .HasOne(cs => cs.Store)
                .WithMany()
                .HasForeignKey(cs => cs.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        private void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
               .HasMany(p => p.Reviews)
               .WithOne(r => r.Product)
               .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Product>()
             .HasOne(p => p.Brand)
             .WithMany(b => b.Products) 
             .HasForeignKey(p => p.BrandId)
             .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Weight).HasColumnType("decimal(10,2)");
                entity.Property(p => p.Width).HasColumnType("decimal(10,2)");
                entity.Property(p => p.Height).HasColumnType("decimal(10,2)");
                entity.Property(p => p.Length).HasColumnType("decimal(10,2)");
            });
        }


        private void ConfigureCart(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }

        private void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>()
                .Property(o => o.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.IsActive)
                .HasDefaultValue(true);
        }

        private void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);
        }


        private void ConfigureStoreCarouselImage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreCarouselImage>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<StoreCarouselImage>()
                .Property(sc => sc.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<StoreCarouselImage>()
                .Property(sc => sc.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

        }

        private void ConfigureAddress(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.ZipCode)
                .IsRequired()
                .HasMaxLength(10);
        }

        private void ConfigureCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Subcategories)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureSubcategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subcategory>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Subcategory>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Subcategory)
                .HasForeignKey(p => p.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
