using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Application.Interfaces;

namespace MovieWebApp.Presentation.Controllers.Admin
{
    [Route("api/admin/dashboard")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IAdminService adminService, ILogger<DashboardController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/Dashboard/stats
        /// Lấy thống kê tổng quan cho Dashboard
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                _logger.LogInformation("Getting dashboard statistics");
                var stats = await _adminService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard statistics");
                return StatusCode(500, new 
                { 
                    message = "Lỗi khi lấy thống kê dashboard", 
                    error = ex.Message 
                });
            }
        }
    }
}
