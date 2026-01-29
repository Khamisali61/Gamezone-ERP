using GameZoneErp.Shared.Entities;

namespace GameZoneErp.Server.Services
{
    public interface IInventoryService
    {
        Task<bool> AdjustStockAsync(int productId, int quantityChange);
        Task<bool> HasSufficientStockAsync(int productId, int quantity);
    }
}
