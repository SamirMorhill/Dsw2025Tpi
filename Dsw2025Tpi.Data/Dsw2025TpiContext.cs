﻿using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext 
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);
        

        modelBuilder.Entity<Product> (eb =>
        {
            eb.ToTable("Products");
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
            /*eb.Property(o => o.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasDefaultValueSql("GETDATE"); //PREGUNTAR SI ESTA BIEN Y CUAL USAMOS*/
            eb.Property(o => o.ShippingAddress)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(o => o.BillingAddress)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(o => o.Note)
            .HasMaxLength(200);
            eb.Property(o => o.Status)
            .HasConversion<string>();
            eb.Ignore(p => p.TotalAmount);
        });

        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.ToTable("Order Item");
            eb.Property(oi => oi.Quantity)
            .HasDefaultValue(0);
            eb.Property(oi => oi.UnitPrice)
            .HasPrecision(15, 2);
            eb.Ignore(oi => oi.SubTotal);
            eb.Property(oi => oi.ProductId)
            .IsRequired()
            .HasMaxLength (100);
        });


        modelBuilder.Entity<Customer>(eb =>
        {
            eb.ToTable("Customer");
            eb.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(60);
            eb.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);
            eb.Property(c => c.PhoneNumber)
            .HasMaxLength(15);
        });

       

    }
}
