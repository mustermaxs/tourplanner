using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tours;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Tourplanner.Entities;
using Tourplanner.Entities.TourLogs.Commands;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IResponse>> SearchInTours([FromQuery] string q)
        {
            var query = new GetSearchResultsQuery(q);

            return await ResponseAsync(query);
        }
    }
}