using GameZoneErp.Shared.Entities;

namespace GameZoneErp.Server.Services
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        decimal CalculateTotal(Sale sale);
    }
}
