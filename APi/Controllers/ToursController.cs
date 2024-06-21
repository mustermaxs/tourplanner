using Api.Entities.Maps;
using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tours;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
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
                var createTourCommand = new CreateTourCommand(
                    createTourDto.Name,
                    createTourDto.Description,
                    createTourDto.From,
                    createTourDto.To,
                    createTourDto.TransportType,
                    createTourDto.Start,
                    createTourDto.Destination
                );
                return await ResponseAsync(createTourCommand);
        }


        [HttpDelete("{tourid}")]
        public async Task<ActionResult<IResponse>> DeleteTour(int tourid)
        {
            var command = new DeleteTourCommand(tourid);
            return await ResponseAsync(command);
        }

        [HttpPut("{tourid}")]   // TODO put this in a single command, use mediator in it to pass it to following commands, pass same unit of work to all commands
        public async Task<ActionResult<IResponse>> UpdateTour([FromBody] UpdateTourDto updateTourDto,
            [FromRoute] int tourid)
        {
            try
            {
                var updateTourCommand = new UpdateTourCommand(
                    tourid,
                    updateTourDto.Name,
                    updateTourDto.Description,
                    updateTourDto.From,
                    updateTourDto.To,
                    updateTourDto.TransportType,
                    updateTourDto.Start,
                    updateTourDto.Destination
                );
                
                return await ResponseAsync(updateTourCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Failed to update tour");
            }
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
        public async Task<ActionResult<IResponse>> DeleteTourLog(int logid)
        {
            var command = new DeleteTourLogCommand(logid);

            return await ResponseAsync(command);
        }

        [HttpPost("import")]
        public async Task<ActionResult<IResponse>> ImportTourFromJsonFile([FromForm] IFormFile file)
        {
            var serializer = new JsonSerializer();
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var tour = (CreateTourDto) serializer.Deserialize(streamReader, typeof(CreateTourDto));
                var command = new CreateTourCommand(
                    tour.Name,
                    tour.Description,
                    tour.From,
                    tour.To,
                    tour.TransportType,
                    tour.Start,
                    tour.Destination
                );
                
                return await ResponseAsync(command);
            }
        }

        [HttpGet("{tourid}/export")]
        public async Task<ActionResult<IResponse>> ExportTourAsJson(int tourid)
        {
            var command = new ExportTourAsJsonCommand(tourid);
            return await ResponseAsync(command);
        }
    }
}