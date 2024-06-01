using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tours;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Tourplanner.Entities;
using Tourplanner.Entities.Tours.Commands;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ReportsController(IMediator mediator) : BaseController(mediator)
    {

        [HttpGet("tours")]
        public async Task<ActionResult<IResponse>> GetSummaryReport()
        {
            var command = new GetSummaryReportRequest();
            return await ResponseAsync(command);
        }

        [HttpGet("tours/{tourid}")]
        public async Task<ActionResult<IResponse>> GetTourReport(int tourid)
        {
            var command = new GetTourReportRequest(tourid);
            return await ResponseAsync(command);
        }
    }
}
