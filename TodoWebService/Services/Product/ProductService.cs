using Microsoft.EntityFrameworkCore;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Product;

public class ProductService(TodoDbContext context) : IProductService
{
    private readonly TodoDbContext _context = context;


    public async Task<ProductItemDto> Create(CreateProductRequest request)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId) ?? throw new NullReferenceException("Category not exist!");
        var product = new ProductItem()
        {
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId,
            Description = request.Description
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return new ProductItemDto(product.Id, product.Name, product.Description, product.Price, product.CategoryId);
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(e => e.Id == id);

        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        else throw new NullReferenceException("Product not found!");
    }

    public async Task<ProductItemDto> Get(int id)
    {
        var product = await _context.Products
                   .FirstOrDefaultAsync(e => e.Id == id);

        return product is not null
            ? new ProductItemDto(product.Id, product.Name, product.Description, product.Price, product.CategoryId)
            : throw new NullReferenceException("Product not found!");
    }

    public async Task<PaginatedListDto<ProductItemDto>> All(PaginationRequest request, string? sortBy, int? categoryId, int? minPrice, int? maxPrice)
    {
        IQueryable<ProductItem> query = _context.Products
                                            .AsQueryable();

        if (sortBy is not null && sortBy.ToLower() == "name")
            query = query.OrderBy(p => p.Name);
        else if (sortBy is not null && sortBy.ToLower() == "price")
            query = query.OrderBy(p => p.Price);

        if (categoryId is not null)
            query = query.Where(p => p.CategoryId == categoryId);

        if (minPrice is not null)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice is not null)
            query = query.Where(p => p.Price <= maxPrice);

        var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();

        return new PaginatedListDto<ProductItemDto>(
            items.Select(e => new ProductItemDto(e.Id, e.Name, e.Description, e.Price, e.CategoryId)),
            new PaginationMeta(request.Page, request.PageSize, totalCount)
            );

    }



}
