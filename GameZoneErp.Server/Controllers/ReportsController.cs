using GameZoneErp.Server.Services;
using GameZoneErp.Shared.DTOs;
using GameZoneErp.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZoneErp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IEndOfDayService _endOfDayService;

        public ReportsController(IEndOfDayService endOfDayService)
        {
            _endOfDayService = endOfDayService;
        }

        [HttpGet("z-report")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ZReportDto>> GetZReport([FromQuery] DateTime? date)
        {
            var reportDate = date ?? DateTime.Now;
            var report = await _endOfDayService.GenerateZReportAsync(reportDate);
            return Ok(report);
        }

        [HttpPost("close-shift")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CloseShift([FromQuery] DateTime? date)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var shiftDate = date ?? DateTime.Now;
            var success = await _endOfDayService.CloseShiftAsync(userId, shiftDate);

            if (!success)
            {
                return BadRequest("Shift already closed or error occurred.");
            }

            return Ok("Shift closed successfully.");
        }
    }
}
