using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Services.Product;

namespace TodoWebService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("get/{id}")]
    public async Task<ActionResult<ProductItemDto>> Get(int id)
    {
        try
        {
            var item = await _productService.Get(id);
            return item is not null
                ? item
                : NotFound();
        }
        catch (NullReferenceException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        try
        {
            return await _productService.Delete(id);

        }
        catch (NullReferenceException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProductItemDto>> Create([FromBody] CreateProductRequest request)
    {
        try
        {
            return await _productService.Create(request);
        }
        catch (NullReferenceException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<PaginatedListDto<ProductItemDto>>> All(PaginationRequest request, string? sortBy, int? categoryId, int? minPrice, int? maxPrice)
    {
        try
        {
            var result = await _productService.All(request, sortBy, categoryId, minPrice, maxPrice);
            return result;
        }
        catch (NullReferenceException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
