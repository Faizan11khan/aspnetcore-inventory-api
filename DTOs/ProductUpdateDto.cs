using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public class ProductUpdateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string ProductName { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "99999999.99")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }
}