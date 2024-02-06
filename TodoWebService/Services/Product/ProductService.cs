using TodoWebService.Data;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Product;

public class ProductService(TodoDbContext context) : IProductService
{
    private readonly TodoDbContext _context = context;

    public Task<List<ProductItemDto>> All(PaginationRequest? request, string? sortBy, Category? category, int? minPrice, int? maxPrice)
    {
        throw new NotImplementedException();
    }

    public Task<ProductItemDto> Create(ProductItemDto request)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductItemDto> Get(int id)
    {
        throw new NotImplementedException();
    }
}
