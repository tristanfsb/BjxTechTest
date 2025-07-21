using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class BjxDbContext : DbContext
{
    public BjxDbContext(DbContextOptions<BjxDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetailOrder> DetailOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetailOrder>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__detail_o__38E9A2244BFA784B");

            entity.ToTable("detail_orders");

            entity.HasIndex(e => new { e.OrderIdFk, e.ProductIdFk }, "uq_order_product").IsUnique();

            entity.Property(e => e.DetailId).HasColumnName("detail_id");
            entity.Property(e => e.OrderIdFk).HasColumnName("order_id_fk");
            entity.Property(e => e.ProductIdFk).HasColumnName("product_id_fk");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.OrderIdFkNavigation).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.OrderIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detail_or__order__403A8C7D");

            entity.HasOne(d => d.ProductIdFkNavigation).WithMany(p => p.DetailOrders)
                .HasForeignKey(d => d.ProductIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__detail_or__produ__412EB0B6");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__465962291E87C39B");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("client_name");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__47027DF503E8543F");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("product_name");
            entity.Property(e => e.Stock).HasColumnName("stock");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F10C7BB55");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616468BFB97F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
