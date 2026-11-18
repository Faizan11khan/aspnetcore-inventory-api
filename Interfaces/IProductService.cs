using InventoryApi.DTOs;

namespace InventoryApi.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllProductsAsync();

    Task<ProductResponseDto?> GetProductByIdAsync(int id);
}