using System;
using System.ComponentModel.DataAnnotations;

namespace GameZoneErp.Shared.Entities
{
    public class ShiftClosure
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int ClosedByUserId { get; set; }

        public decimal TotalSales { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}
