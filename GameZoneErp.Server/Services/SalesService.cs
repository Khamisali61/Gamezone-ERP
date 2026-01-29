using GameZoneErp.Server.Data;
using GameZoneErp.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameZoneErp.Server.Services
{
    public class SalesService : ISalesService
    {
        private readonly GameZoneDbContext _context;
        private readonly IInventoryService _inventoryService;

        public SalesService(GameZoneDbContext context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public decimal CalculateTotal(Sale sale)
        {
            return sale.SaleItems.Sum(i => i.PriceAtSale * i.Quantity);
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            // Calculate total if not set (trust check or enforce)
            if (sale.TotalAmount == 0)
            {
                sale.TotalAmount = CalculateTotal(sale);
            }

            sale.Date = DateTime.Now;

            // Validate and Deduct Stock
            foreach (var item in sale.SaleItems)
            {
                // Verify stock
                if (!await _inventoryService.HasSufficientStockAsync(item.ProductId, item.Quantity))
                {
                    throw new InvalidOperationException($"Insufficient stock for product ID {item.ProductId}");
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                foreach (var item in sale.SaleItems)
                {
                    // Deduct stock (negative quantity change)
                    var success = await _inventoryService.AdjustStockAsync(item.ProductId, -item.Quantity);
                    if (!success)
                    {
                        throw new InvalidOperationException($"Failed to adjust stock for product ID {item.ProductId}. Sale cancelled.");
                    }
                }

                await transaction.CommitAsync();
                return sale;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
