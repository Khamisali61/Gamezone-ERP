using System;

namespace GameZoneErp.Shared.DTOs
{
    public class ZReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public decimal CashSales { get; set; }
        public decimal CardSales { get; set; }
        public decimal MobileMoneySales { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
    }
}
