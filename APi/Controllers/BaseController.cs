using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;
using Microsoft.AspNetCore.Mvc;
using Tourplanner;
using Tourplanner.Exceptions;
using Tourplanner.Infrastructure;

namespace Api.Controllers;
using Api.Services.Logging;
public abstract class BaseController : ControllerBase
{
    private readonly IMediator Mediator;
    protected ILoggerWrapper Logger = LoggerFactory.GetLogger();

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
                Logger.Error($"Response for request {command.GetType().FullName} is null. {command.ToString()}");
                return NotFound();
            }

            if (responseObj is JsonObject)
            {
                var jsonRes = JsonSerializer.Serialize(responseObj);
                Logger.Info($"Response for request {command.GetType().FullName} is {jsonRes}");
                return Content(jsonRes, "application/json");
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
            Logger.Error($"Request {command.GetType().FullName} failed: {rex.Message}. {command.ToString()}");
            return BadRequest(rex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Logger.Error($"Request {command.GetType().FullName} failed.  {command.ToString()}. {e.Message}");
            return Problem($"Something went wrong :(");
        }
    }
}