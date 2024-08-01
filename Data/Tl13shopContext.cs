using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace TL13Shop.Data;

public partial class Tl13shopContext : DbContext
{
    public Tl13shopContext()
    {
    }

    public Tl13shopContext(DbContextOptions<Tl13shopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WishList> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
    
    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-E4IRA0MP;Initial Catalog=TL13Shop;Persist Security Info=True;User ID=sa;Password=1234567;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__DAD4F05EFFC6D773");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandImageUrl).IsUnicode(false);
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B78F8D2252");

            entity.ToTable("Cart");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__ProductId__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cart__UserId__49C3F6B7");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BE6DC2EE0");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259B2809B2C7");

            entity.ToTable("Contact");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF07E798DD");

            entity.Property(e => e.CustomerAddress).IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Orders__ProductI__52593CB8");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Orders__StatusId__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserId__5165187F");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__C8EE2063B06F6559");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CD59E661E6");

            entity.ToTable("Product");

            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.LongDescription).IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(20, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Product__BrandId__300424B4");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__30F848ED");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__7516F70C179C0EC2");

            entity.ToTable("ProductImage");

            entity.Property(e => e.ImageUrl).IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductIm__Produ__33D4B598");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__ProductR__74BC79CE632FB52D");

            entity.ToTable("ProductReview");

            entity.Property(e => e.Comment).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductRe__Produ__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProductRe__UserI__4222D4EF");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A60634CB0");

            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB9744E8F");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F2845610C24D25").IsUnique();

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl).IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RandomKey).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleId__3E52440B");
        });

        modelBuilder.Entity<WishList>(entity =>
        {
            entity.HasKey(e => e.WishListId).HasName("PK__WishList__E41F8787D70EC142");

            entity.ToTable("WishList");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__WishList__Produc__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.WishLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__WishList__UserId__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
