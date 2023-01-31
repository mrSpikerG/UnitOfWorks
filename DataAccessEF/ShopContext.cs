using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccessEF;

public partial class ShopContext : IdentityDbContext<IdentityUser> {
    public ShopContext(){}

    public ShopContext(DbContextOptions options): base(options){}
   


    public virtual DbSet<Category> Categories { get; set; }

    //public virtual DbSet<CategoryConnection> CategoryConnections { get; set; }

    //public virtual DbSet<ShopItem> ShopItems { get; set; }

   
   
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(true);
        });

        //modelBuilder.Entity<CategoryConnection>(entity =>
        //{
        //    entity.ToTable("CategoryConnection");
        //});


        //modelBuilder.Entity<ShopItem>(entity =>
        //{
        //    entity.ToTable("ShopItem");

        //    entity.Property(e => e.Name)
        //        .HasMaxLength(100)
        //        .IsUnicode(false);
        //    entity.Property(e => e.Price).HasColumnType("money");
        //});

       

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
