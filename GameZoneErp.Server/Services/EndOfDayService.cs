using GameZoneErp.Server.Data;
using GameZoneErp.Shared.DTOs;
using GameZoneErp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameZoneErp.Server.Services
{
    public class EndOfDayService : IEndOfDayService
    {
        private readonly GameZoneDbContext _context;

        public EndOfDayService(GameZoneDbContext context)
        {
            _context = context;
        }

        public async Task<ZReportDto> GenerateZReportAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);

            var sales = await _context.Sales
                .Where(s => s.Date >= startOfDay && s.Date <= endOfDay)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.Date >= startOfDay && e.Date <= endOfDay)
                .SumAsync(e => e.Amount);

            var report = new ZReportDto
            {
                Date = startOfDay,
                TotalSales = sales.Sum(s => s.TotalAmount),
                CashSales = sales.Where(s => s.PaymentMethod == PaymentMethod.Cash).Sum(s => s.TotalAmount),
                CardSales = sales.Where(s => s.PaymentMethod == PaymentMethod.Card).Sum(s => s.TotalAmount),
                MobileMoneySales = sales.Where(s => s.PaymentMethod == PaymentMethod.MobileMoney).Sum(s => s.TotalAmount),
                TotalExpenses = expenses
            };

            report.NetProfit = report.TotalSales - report.TotalExpenses; // Simplified P&L (Sales - Expenses). COGS would require Product Cost calculation.

            return report;
        }

        public async Task<bool> CloseShiftAsync(int userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);

            // Check if already closed
            var existingClosure = await _context.ShiftClosures
                .FirstOrDefaultAsync(sc => sc.Date >= startOfDay && sc.Date <= endOfDay);

            if (existingClosure != null)
            {
                return false; // Already closed
            }

            var report = await GenerateZReportAsync(date);

            var closure = new GameZoneErp.Shared.Entities.ShiftClosure
            {
                Date = DateTime.Now,
                ClosedByUserId = userId,
                TotalSales = report.TotalSales,
                TotalExpenses = report.TotalExpenses
            };

            _context.ShiftClosures.Add(closure);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
