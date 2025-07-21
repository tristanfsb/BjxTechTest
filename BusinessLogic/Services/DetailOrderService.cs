using System;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SharedLib.DTOs;

namespace BusinessLogic.Services;

public class DetailOrderService(BjxDbContext context, ILogger<DetailOrderService> logger)
{
    private readonly BjxDbContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<DetailOrder?> SaveDetailOrder(DetailOrderDto detailOrderDto)
    {
        try
        {
            var detailOrder = detailOrderDto.ToEntity();

            var product = await _context.Products.Where(p => p.ProductId == detailOrder.ProductIdFk).FirstOrDefaultAsync();

            //Verifica que el producto exista
            if (product is null)
            {
                return null;
            }
            //Verifica que el stock sea mayor a la cantidad del pedido
            if (product.Stock < detailOrder.Quantity)
            {
                return null;
            }
            //Resta la cantidad al stock
            product.Stock -= detailOrder.Quantity;

            //Agrega el detalle y guarda los cambios
            await _context.DetailOrders.AddAsync(detailOrder);
            await _context.SaveChangesAsync();

            return detailOrder;
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }

    public async Task<IList<DetailOrder>?> GetDetailOrder(int orderIdFk)
    {
        try
        {
            return await _context.DetailOrders.Include(d => d.ProductIdFkNavigation).Where(d => d.OrderIdFk == orderIdFk).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"[ERROR]: {ex.Message} - {ex.StackTrace}");
            return null;
        }
    }
}
