using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext 
{
    
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);
        

        modelBuilder.Entity<Product> (eb =>
        {
            eb.ToTable("Product");
            eb.Property(p => p.Sku)
            .HasMaxLength(20)
            .IsRequired();
            eb.Property(p => p.InternalCode)
            .HasMaxLength(20);
            eb.Property(p => p.Name)
            .HasMaxLength(60);
            eb.Property(p => p.Description)
            .HasMaxLength(200);
            eb.Property(p => p.CurrentUnitPrice)
            .HasPrecision(15, 2);
            eb.Property(p => p.StockQuantity)
            .HasDefaultValue(0);
        });


        modelBuilder.Entity<Order>(eb =>
        {
            eb.ToTable("Order");
            eb.Property(p => p.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasDefaultValueSql("GETDATE"); //PREGUNTAR SI ESTA BIEN Y CUAL USAMOS
            eb.Property(p => p.ShippingAddress)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(p => p.BillingAddress)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(p => p.Note)
            .HasMaxLength(200);
            eb.Property(p => p.TotalAmount)
            .IsRequired()
            .HasPrecision(15, 2);
        });


        modelBuilder.Entity<Customer>(eb =>
        {
            eb.ToTable("Customer");
            eb.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(60);
            eb.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(p => p.PhoneNumber)
            .HasMaxLength(15);
        });

       

    }
}
