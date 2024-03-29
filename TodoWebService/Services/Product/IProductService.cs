﻿using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Product;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services.Product;

public interface IProductService
{
    Task<ProductItemDto> Get(int id);
    Task<bool> Delete(int id);
    Task<ProductItemDto> Create(CreateProductRequest request);
    Task<PaginatedListDto<ProductItemDto>> All(PaginationRequest request, string? sortBy, int? categoryId,int? minPrice,int? maxPrice);
}