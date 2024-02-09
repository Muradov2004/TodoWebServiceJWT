using System.ComponentModel.DataAnnotations;

namespace TodoWebService.Models.DTOs.Product;

public class CreateProductRequest
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MinLength(8)]
    public string Description { get; set; } = string.Empty;
    [Required]
    [Range(0, 1000000)]
    public decimal Price { get; set; }
    [Required]
    public int CategoryId { get; set; }
}
