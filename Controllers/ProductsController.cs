using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.DTOs;
using InventoryApi.Interfaces;  

namespace InventoryApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly IProductService _productService;

    public ProductsController(
        InventoryDbContext context,
        IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct(
     [FromBody] ProductCreateDto request)
    {
        var product = await _productService.CreateProductAsync(request);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.ProductId },
            product
        );
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(
      int id,
      [FromBody] ProductUpdateDto request)
    {
        var updated = await _productService.UpdateProductAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }
    [HttpGet("find-by-name")]
    public async Task<ActionResult<Product>> GetProductByName(string name)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductName == name);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Product>>> FilterProducts(
    [FromQuery] string? category,
    [FromQuery] decimal? minPrice)
    {
        IQueryable<Product> query =
            _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        var products = await query
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return Ok(products);
    }
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(
        [FromQuery] string? name)
    {
        IQueryable<Product> query = _context.Products.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.ProductName.Contains(name));
        }
        var products = await query.ToListAsync();
        return Ok(products);
    }
    [HttpGet("cheapest")]
    public async Task<ActionResult<Product>> GetCheapestProduct()
    {
        var product = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Price)
            .ThenBy(p => p.ProductId)
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return NotFound("No products found.");
        }

        return Ok(product);
    }
    [HttpGet("sort-by-price")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsSortedByPrice()
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderByDescending(p => p.Price)
            .ToListAsync();

        return Ok(products);
    }
    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsPaged(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 2)
    {
        if (pageNumber < 1)
        {
            return BadRequest("Page number must be at least 1.");
        }

        if (pageSize < 1 || pageSize > 50)
        {
            return BadRequest("Page size must be between 1 and 50.");
        }

        int skip = (pageNumber - 1) * pageSize;

        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return Ok(products);
    }
}