using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Models;
using Tourplanner.Entities.Tour;
using Tourplanner.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ToursController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ToursController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try {
            
                var command = new GetTourCommand();
                
                var result = await _mediator.Send(command);

                return Ok(result);

            } catch (Exception e) { 
                    return BadRequest(e.Message);
            }
        }

        // GET api/<ToursController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            try {
                var command = new GetTourCommand(id);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        // POST api/<ToursController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            try {
                var command = new CreateTourCommand(value);
                var result = _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<ToursController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] string value)
        {
            try {
                var command = new UpdateTourCommand(id, value);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<ToursController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try {
                var command = new DeleteTourCommand(id);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}