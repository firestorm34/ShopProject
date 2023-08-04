using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShopProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#nullable disable

namespace ShopProject.Data

{
    public partial class ShopContext :  IdentityDbContext<User, Role, int>
    {
        IConfiguration Configuration;
        public ShopContext(IConfiguration configuration, DbContextOptions<ShopContext> options)
            : base(options)
        { 
        
        }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<GoodAtStock> GoodAtStocks { get; set; }
        public virtual DbSet<GoodInBasket> GoodInBaskets { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<HistoryElement> HistoryElements { get; set; }
        public virtual DbSet<LikedGood> LikedGoods { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=Password12;database=shop", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.17-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            

            modelBuilder.Entity<Basket>(entity =>
            {
                entity.ToTable("baskets");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AmountOfGood).HasColumnType("int(11)");

                entity.Property(e => e.TotalSum).HasPrecision(20, 6);

                entity.HasOne(d => d.User)
                  .WithOne(p => p.Basket)
                  .HasForeignKey<Basket>(d => d.UserId)
                  .HasConstraintName("FK_baskets_users");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasIndex(e => e.ParentCategoryId, "FK_categories_to_parent_categories");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.ParentCategoryId).HasColumnType("int(11)").HasDefaultValue("0");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_categories_to_parent_categories");
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.ToTable("goods");

                entity.HasIndex(e => e.CategoryId, "FK_goods_categories");

                entity.HasIndex(e => e.ManufacturerId, "FK_goods_manufacturers");

                entity.Property(e => e.Id).HasColumnType("int(11)");
                
               
                 

                entity.Property(e => e.CategoryId).HasColumnType("int(11)");

                entity.Property(e => e.ManufacturerId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Price).HasPrecision(20, 6);

                entity.Property(e => e.Year).HasColumnType("int(11)");

                entity.Property<bool>(e => e.IsArchieved).HasColumnType("tinyint(1)").HasDefaultValue("false");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_goods_categories");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Goods)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_goods_manufacturers");

            });

            modelBuilder.Entity<GoodAtStock>(entity =>
            {
                entity.ToTable("goodatstocks");

                entity.HasIndex(e => e.GoodId, "FK_goodatstocks_goods");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AmountLeft).HasColumnType("int(11)");

                entity.Property(e => e.GoodId).HasColumnType("int(11) ");

                entity.HasOne(d => d.Good)
                    .WithOne(p => p.GoodAtStock)
                    .HasForeignKey<GoodAtStock>(d => d.GoodId)
                    .HasConstraintName("FK_goodatstocks_goods");
            });

            modelBuilder.Entity<GoodInBasket>(entity =>
            {
                entity.ToTable("goodinbaskets");

                entity.HasIndex(e => e.BasketId, "FK_goodinbaskets_baskets");

                entity.HasIndex(e => e.GoodId, "FK_goodinbaskets_goods");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnType("int(11)");

                entity.Property(e => e.BasketId).HasColumnType("int(11) ");

                entity.Property(e => e.GoodId).HasColumnType("int(11) ");

                entity.Property(e => e.SumOfGoods).HasPrecision(20, 6);

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.GoodInBaskets)
                    .HasForeignKey(d => d.BasketId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_goodinbaskets_baskets");

                entity.HasOne(d => d.Good)
                    .WithMany(p => p.GoodInBaskets)
                    .HasForeignKey(d => d.GoodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_goodinbaskets_goods");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("histories");

                entity.HasIndex(e => e.UserId, "FK_histories_users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AmountOfViwedGoods).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Histories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_histories_users");
            });

            modelBuilder.Entity<HistoryElement>(entity =>
            {
                entity.ToTable("historyelements");

                entity.HasIndex(e => e.ViwedGoodId, "FK_historyelements_goods");

                entity.HasIndex(e => e.HistoryId, "FK_historyelements_histories");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.HistoryId).HasColumnType("int(11)");

                entity.Property(e => e.ViwedGoodId).HasColumnType("int(11) ");

                entity.HasOne(d => d.History)
                    .WithMany(p => p.HistoryElements)
                    .HasForeignKey(d => d.HistoryId)
                    .HasConstraintName("FK_historyelements_histories");

                entity.HasOne(d => d.ViwedGood)
                    .WithMany(p => p.HistoryElements)
                    .HasForeignKey(d => d.ViwedGoodId)
                    .HasConstraintName("FK_historyelements_goods");
            });

            modelBuilder.Entity<LikedGood>(entity =>
            {
                entity.ToTable("likedgoods");

                entity.HasIndex(e => e.GoodId, "FK_likedgoods_goods");

                entity.HasIndex(e => e.UserId, "FK_likedgoods_users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.GoodId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Good)
                    .WithMany(p => p.LikedGoods)
                    .HasForeignKey(d => d.GoodId)
                    .HasConstraintName("FK_likedgoods_goods");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LikedGoods)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_likedgoods_users");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("manufacturers");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.BasketId, "FK_orders_baskets");

                entity.HasIndex(e => e.UserId, "FK_orders_users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Adress).HasColumnType("int(11)");

                entity.Property(e => e.BasketId).HasColumnType("int(11) ");

                entity.Property(e => e.Cost).HasPrecision(20, 6);

                entity.Property(e => e.Status).HasColumnType("enum('Delivered','InProcess')");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Basket)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BasketId)
                    .HasConstraintName("FK_orders_baskets");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_orders_users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
