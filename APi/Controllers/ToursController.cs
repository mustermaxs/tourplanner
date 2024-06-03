﻿using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tours;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Tourplanner.Entities;
using Tourplanner.Entities.TourLogs.Commands;

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
                createTourDto.TransportType
            );


            // var saveMapImageCommand = new SaveMapImageCommand();
            // await mediator.Send(saveMapImageCommand);
            
            return await ResponseAsync(createTourCommand);
        }
        
        // [HttpGet]
        // public async Task<string>
        //
        // [HttpGet("search")]
        // public async Task<ActionResult<IResponse>> SearchInTours([FromBody] SearchDto searchDto)
        // {
        //     var query = new GetSearchResultsQuery(searchDto.SearchTerm);
        // }

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
                createTourLogDto.TotalTime,
                createTourLogDto.Rating
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
                TotalTime: updateTourLogDto.TotalTime,
                Rating: updateTourLogDto.Rating);

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