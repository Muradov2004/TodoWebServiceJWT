using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Services.Product;

namespace TodoWebService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<ProductItemDto>> Get(int id)
    {
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        return Ok();
    }

    [HttpPost("create")]
    public async Task<ActionResult<ProductItemDto>> Create([FromBody] ProductItemDto request)
    {
        return Ok();
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<ProductItemDto>>> All()
    {
        return Ok();
    }
    
}
