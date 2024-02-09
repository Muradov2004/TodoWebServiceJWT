namespace TodoWebService.Models.DTOs.Product;

public class ProductItemDto(int id, string name, string description, decimal price, int categoryId)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal Price { get; set; } = price;
    public int CategoryId { get; set; } = categoryId;
}