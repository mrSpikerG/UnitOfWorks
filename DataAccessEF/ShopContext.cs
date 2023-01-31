using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessEF;

public partial class ShopContext : DbContext
{
    public ShopContext(){}

    public ShopContext(DbContextOptions options): base(options){}


    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryConnection> CategoryConnections { get; set; }

    public virtual DbSet<ShopItem> ShopItems { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CategoryConnection>(entity =>
        {
            entity.ToTable("CategoryConnection");
        });


        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.ToTable("ShopItem");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC07DDBE17BA");

            entity.ToTable("UserInfo");

            entity.Property(e => e.ApiKey)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
