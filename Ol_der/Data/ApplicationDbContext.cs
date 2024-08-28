using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ol_der.Models;

namespace Ol_der.Data
{
    class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .Property(s => s.SaleId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Sale>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.SaleItemId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<SaleItem>()
                .Property(si => si.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasIndex(o => new { o.SupplierId, o.OrderDate, o.IsOpen })
                .HasDatabaseName("IDX_Orders_SupplierId_OrderDate_IsOpen");
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OrderItemId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Supplier>()
                .Property(s => s.SupplierId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Warranty>()
                .Property(w => w.WarrantyId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WarrantyStatus>()
                .Property(w => w.WarrantyStatusId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Note>()
                .Property(n => n.NoteId)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<CustomerOrder>()
                .Property(co => co.CustomerOrderId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CustomerOrderStatus>()
                .Property(cos => cos.CustomerOrderStatusId)
                .ValueGeneratedOnAdd();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<WarrantyStatus> WarrantyStatuses { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderStatus> CustomerOrderStatuses { get; set; }

    }
}
