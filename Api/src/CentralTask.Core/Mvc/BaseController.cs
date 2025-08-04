using CentralTask.Core.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CentralTask.Core.Mvc;

public abstract class BaseController : Controller
{
    protected IActionResult HandleResult(IMediatorResult result)
    {
        if (result.Exception is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { result.IsValid, result.Exception.Message });
        }

        return result.IsValid
            ? Ok(result)
            : BadRequest(new { result.IsValid, result.Errors });
    }
}