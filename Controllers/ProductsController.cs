using DevOpsDemo.Api.Data;
using DevOpsDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevOpsDemo.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        => Ok(await db.Products.OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var product = await db.Products.FindAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        product.Id = 0;
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product input)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.Name = input.Name;
        product.Description = input.Description;
        product.Price = input.Price;
        product.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
