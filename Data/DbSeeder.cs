using DevOpsDemo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DevOpsDemo.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Products.AnyAsync())
            return;

        db.Products.AddRange(
            new Product { Name = "Laptop", Description = "Demo laptop", Price = 50000 },
            new Product { Name = "Monitor", Description = "24 inch monitor", Price = 15000 },
            new Product { Name = "Keyboard", Description = "Mechanical keyboard", Price = 2500 }
        );

        await db.SaveChangesAsync();
    }
}
