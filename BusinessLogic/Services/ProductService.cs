using System;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BusinessLogic.Services;

public class ProductService(BjxDbContext context, ILogger<ProductService> logger)
{

    private readonly BjxDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<Product?> SaveProduct(ProductDto productDto)
    {
        try
        {
            var product = productDto.ToEntity();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

    public async Task<IList<Product>?> GetProducts()
    {
        try
        {
            return await _context.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

}
