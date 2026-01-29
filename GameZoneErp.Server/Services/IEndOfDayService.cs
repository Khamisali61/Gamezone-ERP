using GameZoneErp.Shared.DTOs;

namespace GameZoneErp.Server.Services
{
    public interface IEndOfDayService
    {
        Task<ZReportDto> GenerateZReportAsync(DateTime date);
        Task<bool> CloseShiftAsync(int userId, DateTime date);
    }
}
