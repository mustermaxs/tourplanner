using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tour;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLog;
using Tourplanner.Infrastructure;
using Microsoft.AspNetCore.Cors;

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

        [HttpPost]
        public async Task<ActionResult<IResponse>> CreateTour([FromBody] CreateTourCommand createTourCommand)
        {
            return await ResponseAsync(createTourCommand);
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

        [HttpGet("logs/{logid}")]
        public async Task<ActionResult<IResponse>> GetLog(int logid)
        {
            var command = new GetSingleTourLogRequest(logid);
            return await ResponseAsync(command);
        }

        [HttpPost("{tourid}/logs")]
        public async Task<ActionResult<IResponse>> CreateTourLog([FromBody] CreateTourLogDto createTourLogDto, int tourid)
        {
            var command = new CreateTourLogCommand(
                tourid,
                DateTime.UtcNow,
                createTourLogDto.Comment,
                createTourLogDto.Difficulty,
                createTourLogDto.TotalTime,
                createTourLogDto.Rating
            );

            return await ResponseAsync(command);
        }

        [HttpPut("logs/{logid}")]
        public async Task<ActionResult<IResponse>> CreateTourLog([FromBody] UpdateTourLogDto updateTourLogDto, int logid)
        {
            var command = new UpdateTourLogCommand(
                TourLogId: logid,
                DateTime: DateTime.UtcNow, 
                Comment: updateTourLogDto.Comment,
                Difficulty: updateTourLogDto.Difficulty,
                TotalTime: updateTourLogDto.TotalTime,
                Rating: updateTourLogDto.Rating);

            return await ResponseAsync(command);
        }


        [HttpDelete("logs/{logid}")]
        public async Task<ActionResult<IResponse>> DeleteTourLog([FromBody] DeleteTourLogCommand deleteTourLogCommand, int logid)
        {
            var command = new DeleteTourLogCommand(logid);

            return await ResponseAsync(command);
        }
    }
}