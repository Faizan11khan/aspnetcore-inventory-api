using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services;

public class ProductService : IProductService
{
    private readonly InventoryDbContext _context;

    public ProductService(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category
            })
            .ToListAsync();

        return products;
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.ProductId == id)
            .Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category
            })
            .FirstOrDefaultAsync();

        return product;
    }
}