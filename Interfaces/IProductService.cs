using InventoryApi.DTOs;

namespace InventoryApi.Interfaces;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllProductsAsync();

    Task<ProductResponseDto?> GetProductByIdAsync(int id);

    Task<ProductResponseDto> CreateProductAsync(ProductCreateDto request);

    Task<bool> UpdateProductAsync(int id, ProductUpdateDto request);

    Task<bool> DeleteProductAsync(int id);
}