using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tour;
using Tourplanner.DTOs;
using Tourplanner.Infrastructure;

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
    }
}