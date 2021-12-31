using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Models;

namespace TOTVS.WebApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [NonAction]
        protected IActionResult HandleUnsuccessfulResult(List<ErrorModel> errors)
        {
            var error = errors.First();
            return new ObjectResult(new ErrorMessage(error.ErrorMessage)) { StatusCode = error.ErrorStatusCode };
        }
    }
}
