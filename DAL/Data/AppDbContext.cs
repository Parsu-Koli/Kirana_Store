using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // ------------------- USERS -------------------
        public DbSet<User> Users { get; set; }

        // ------------------- STOCKS -------------------
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // ------------------- MASTER TABLES -------------------
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        // ------------------- SUPPLIER -------------------
        public DbSet<Supplier> Suppliers { get; set; }

        // ------------------- PURCHASE -------------------
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        // ------------------- CUSTOMER -------------------
        public DbSet<Customer> Customers { get; set; }

        // ------------------- SALES (BILLING) -------------------
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        // ------------------- STOCK LEDGER -------------------
        public DbSet<StockLedger> StockLedgers { get; set; }

        // ------------------- PAYMENT -------------------
        public DbSet<Payment> Payments { get; set; }

        // ------------------- AUDIT LOG -------------------
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------- User Table --------
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            // -------- Category --------
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            // -------- Product --------
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------- Supplier --------
            modelBuilder.Entity<Supplier>()
                .HasKey(s => s.SupplierId);

            // -------- Purchase --------
            modelBuilder.Entity<Purchase>()
                .HasKey(p => p.PurchaseId);

            modelBuilder.Entity<Purchase>()
                .HasOne<Supplier>()
                .WithMany()
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------- PurchaseItem --------
            modelBuilder.Entity<PurchaseItem>()
                .HasKey(pi => pi.PurchaseItemId);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne<Purchase>()
                .WithMany()
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------- Customer --------
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId);

            // -------- Sale --------
            modelBuilder.Entity<Sale>()
                .HasKey(s => s.SaleId);

            modelBuilder.Entity<Sale>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // -------- SaleItem --------
            modelBuilder.Entity<SaleItem>()
                .HasKey(si => si.SaleItemId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)                 
                .WithMany(s => s.SaleItems)            
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany() 
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(); 
       

            // -------- Stock Ledger --------
            modelBuilder.Entity<StockLedger>()
                .HasKey(sl => sl.Id);

            modelBuilder.Entity<StockLedger>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(sl => sl.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------- Payment --------
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.PaymentId);

            modelBuilder.Entity<Payment>()
                .HasOne<Sale>()
                .WithMany()
                .HasForeignKey(p => p.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------- AuditLog --------
            modelBuilder.Entity<AuditLog>()
                .HasKey(a => a.AuditId);
        }
    }
}
