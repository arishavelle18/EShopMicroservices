using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext : DbContext
{
    public DiscountContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = "75924CFB-EB28-4016-89B6-FAA77426B2BD",
                ProductName = "Discounted Keyboard",
                Description = "Mechanical keyboard promo",
                Amount = 10
            },
            new Coupon
            {
                Id = "021572B8-F35F-4BAE-B806-B076AFA1263D",
                ProductName = "Wireless Mouse",
                Description = "20% off wireless mouse",
                Amount = 20
            },
            new Coupon
            {
                Id = "498C900E-3EFD-4988-8135-F3E849C8EF34",
                ProductName = "USB-C Cable",
                Description = "Buy 1 take 1 USB-C cable",
                Amount = 5
            }

            );
    }

}
