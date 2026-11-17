namespace InventoryApi.DTOs
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Category {  get; set; }
    }
}
