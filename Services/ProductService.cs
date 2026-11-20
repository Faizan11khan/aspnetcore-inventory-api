using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Interfaces;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ProductResponseDto> CreateProductAsync(
    ProductCreateDto request)
    {
        var product = new Product
        {
            ProductName = request.ProductName,
            Price = request.Price,
            Stock = request.Stock,
            Category = request.Category
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return new ProductResponseDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category
        };
    }
    public async Task<bool> UpdateProductAsync(
    int id,
    ProductUpdateDto request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        product.ProductName = request.ProductName;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Category = request.Category;

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return true;
    }
}