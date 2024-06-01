using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Tourplanner.Exceptions;
using Tourplanner.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly IMigrationService _migrationService;

        public MigrationController(IMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpGet("export/{tourId}")]
        public async Task<IActionResult> ExportTour(int tourId)
        {
            try
            {
                var data = await _migrationService.ExportTour(tourId);
                return File(data, "application/json", $"tour_{tourId}.json");
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTour(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                await _migrationService.ImportTour(stream.ToArray());
            }

            return Ok("Tour imported successfully");
        }
    }
}
