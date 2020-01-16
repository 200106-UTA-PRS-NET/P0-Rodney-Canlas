using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzaBox.Domain.Models
{
    public partial class StoreDBContext : DbContext
    {
        public StoreDBContext()
        {
        }

        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<UserOrder> UserOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-8AEKDMJ\\SQLEXPRESS;Database=StoreDB;trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Account__536C85E53F1AD4B5");

                entity.ToTable("Account", "PizzaBox");

                entity.Property(e => e.Username)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Inventory", "PizzaBox");

                entity.Property(e => e.CheesePizza).HasDefaultValueSql("((500))");

                entity.Property(e => e.ChickenTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.JalapenoTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.MarinaraSauce).HasDefaultValueSql("((500))");

                entity.Property(e => e.PepperoniPizza).HasDefaultValueSql("((500))");

                entity.Property(e => e.PepperoniTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.PineappleTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.SausageTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.ThickCrust).HasDefaultValueSql("((500))");

                entity.Property(e => e.ThinCrust).HasDefaultValueSql("((500))");

                entity.Property(e => e.VegetarianPizza).HasDefaultValueSql("((500))");

                entity.Property(e => e.VeggieTopping).HasDefaultValueSql("((500))");

                entity.Property(e => e.WhiteGarlicSauce).HasDefaultValueSql("((500))");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_LocationIDInventory");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "PizzaBox");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__UserOrde__C3905BAF29314A6E");

                entity.ToTable("UserOrder", "PizzaBox");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomPizza).HasDefaultValueSql("((0))");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.PresetPizza).HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalCost).HasColumnType("money");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.UserOrder)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_LocationID");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.UserOrder)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
