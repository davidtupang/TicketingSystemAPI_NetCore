using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Data;

namespace TicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConnectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestConnectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("checkDatabaseConnection")]
        public IActionResult CheckConnection()
        {
            try
            {
                var canConnect = _context.Database.CanConnect();
                if (canConnect)
                {
                    return Ok("Connection to database Success!");
                }
                else
                {
                    return StatusCode(500, "Error connection to database.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Exception Message: {ex.Message}");
            }
        }
    }
}
