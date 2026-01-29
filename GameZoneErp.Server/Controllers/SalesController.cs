using GameZoneErp.Server.Data;
using GameZoneErp.Server.Services;
using GameZoneErp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameZoneErp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly GameZoneDbContext _context;

        public SalesController(ISalesService salesService, GameZoneDbContext context)
        {
            _salesService = salesService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sale>>> GetSales()
        {
            return await _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            try
            {
                var createdSale = await _salesService.CreateSaleAsync(sale);
                return CreatedAtAction("GetSales", new { id = createdSale.Id }, createdSale);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
