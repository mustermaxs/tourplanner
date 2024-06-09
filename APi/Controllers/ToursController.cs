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
using Tourplanner.Entities.Maps;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToursController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<IResponse>> GetTours()
        {
            var command = new GetToursRequest();
            return await ResponseAsync(command);
        }

        [HttpGet("{tourid}")]
        public async Task<ActionResult<IResponse>> GetTourById(int tourid)
        {
            var command = new GetTourByIdRequest(tourid);
            return await ResponseAsync(command);
        }

        [HttpGet("{tourId}/map")]
        public async Task<ActionResult<IResponse>> GetMapForTour(int tourId)
        {
            var request = new GetMapForTourRequest(tourId);
            return await ResponseAsync(request);
        }

        [HttpGet("geosuggestion")]
        public async Task<ActionResult<IResponse>> GetAutoCompleteSuggestionForLocation([FromQuery] string location)
        {
            var query = new GetGeoAutoCompleteQuery(location);
            return await ResponseAsync(query);
        }

        [HttpPost]
        public async Task<ActionResult<IResponse>> CreateTour([FromBody] CreateTourDto createTourDto)
        {
            try
            {
                var createTourCommand = new CreateTourCommand(
                    createTourDto.Name,
                    createTourDto.Description,
                    createTourDto.From,
                    createTourDto.To,
                    createTourDto.TransportType,
                    createTourDto.Start,
                    createTourDto.Destination
                );
                
                var tourId = Convert.ToInt32(await mediator.Send(createTourCommand));
                var createMapCommand = new CreateMapCommand(tourId, createTourDto.Start, createTourDto.Destination, createTourDto.TransportType);
                await mediator.Send(createMapCommand);
                        
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{tourid}")]
        public async Task<ActionResult<IResponse>> DeleteTour(int tourid)
        {
            var command = new DeleteTourCommand(tourid);
            return await ResponseAsync(command);
        }

        [HttpPut("{tourid}")]
        public async Task<ActionResult<IResponse>> UpdateTour([FromBody] UpdateTourDto updateTourDto,
            [FromRoute] int tourid)
        {
            var command = new UpdateTourCommand(
                tourid,
                updateTourDto.Name,
                updateTourDto.Description,
                updateTourDto.From,
                updateTourDto.To,
                updateTourDto.TransportType
            );

            return await ResponseAsync(command);
        }

        [HttpGet("{tourid}/logs")]
        public async Task<ActionResult<IResponse>> GetLogsForTour(int tourid)
        {
            var command = new GetTourLogsRequest(tourid);
            return await ResponseAsync(command);
        }
        
        [HttpGet("{tourid}/tiles")] // TODO implement
        public async Task<ActionResult<IResponse>> GetTilesForTour(int tourid)
        {
            throw new NotImplementedException();
        }

        [HttpGet("logs/{logid}")]
        public async Task<ActionResult<IResponse>> GetLog(int logid)
        {
            var command = new GetSingleTourLogRequest(logid);
            return await ResponseAsync(command);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<IResponse>> SearchInTours([FromQuery] string q)
        {
            var query = new GetSearchResultsQuery(q);

            return await ResponseAsync(query);
        }

        [HttpPost("{tourid}/logs")]
        public async Task<ActionResult<IResponse>> CreateTourLog([FromBody] CreateTourLogDto createTourLogDto,
            int tourid)
        {
            var command = new CreateTourLogCommand(
                tourid,
                DateTime.UtcNow,
                createTourLogDto.Comment,
                createTourLogDto.Difficulty,
                createTourLogDto.Rating,
                createTourLogDto.Duration,
                createTourLogDto.Distance
            );

            return await ResponseAsync(command);
        }

        [HttpPut("logs/{logid}")]
        public async Task<ActionResult<IResponse>> CreateTourLog([FromBody] UpdateTourLogDto updateTourLogDto,
            int logid)
        {
            var command = new UpdateTourLogCommand(
                TourLogId: logid,
                DateTime: DateTime.UtcNow,
                Comment: updateTourLogDto.Comment,
                Difficulty: updateTourLogDto.Difficulty,
                Rating: updateTourLogDto.Rating,
                Duration: updateTourLogDto.Duration,
                Distance: updateTourLogDto.Distance
                );

            return await ResponseAsync(command);
        }


        [HttpDelete("logs/{logid}")]
        public async Task<ActionResult<IResponse>> DeleteTourLog([FromBody] DeleteTourLogCommand deleteTourLogCommand,
            int logid)
        {
            var command = new DeleteTourLogCommand(logid);

            return await ResponseAsync(command);
        }
    }
}