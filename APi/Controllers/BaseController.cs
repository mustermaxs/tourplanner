using System.Windows.Input;
using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Exceptions;
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
            Console.WriteLine(command.ToString());
            var responseObj = await Mediator.Send(command);
            if (responseObj is null)
            {
                return NotFound();
            }

            if (responseObj is byte[] pdfBytes)
            {
                return File(pdfBytes, "application/pdf");
            }

            return Ok(responseObj);
        }
        catch (ResourceNotFoundException rex)
        {
            Console.WriteLine(rex);
            return BadRequest(rex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem($"Something went wrong :(");
        }
    }
}