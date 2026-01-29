using GameZoneErp.Server.Data;
using GameZoneErp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameZoneErp.Server.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly GameZoneDbContext _context;

        public InventoryService(GameZoneDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AdjustStockAsync(int productId, int quantityChange)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            // Service items don't track stock
            if (product.Type == ProductType.Service) return true;

            // Check if we have enough stock for reduction
            if (quantityChange < 0 && product.StockQuantity + quantityChange < 0)
            {
                return false;
            }

            product.StockQuantity += quantityChange;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasSufficientStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            if (product.Type == ProductType.Service) return true;

            return product.StockQuantity >= quantity;
        }
    }
}
