using System.Windows.Input;
using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Infrastructure;

namespace Api.Controllers;

public abstract class BaseController : ControllerBase
{
    private readonly IMediator Mediator;

    public BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected async Task<ActionResult<IResponse>> ResponseAsync(IRequest command)
    {
        try
        {
            var responseObj = await Mediator.Send(command);
            
            if (responseObj is null)
            {
                return NotFound();
            }

            // return responseObj.IsOk ? Ok(responseObj) : BadRequest();
            return Ok(responseObj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem($"Something went wrong :(");
        }
    }
}