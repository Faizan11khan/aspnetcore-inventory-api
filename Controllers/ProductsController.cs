using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController:ControllerBase
{
    private readonly InventoryDbContext _context;
    
    public ProductsController(InventoryDbContext context)
    {
        _context = context;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(
        [FromBody]Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.ProductId },
            product);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
}
