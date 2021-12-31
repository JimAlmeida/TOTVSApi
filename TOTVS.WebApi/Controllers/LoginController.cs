using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Entities;
using TOTVS.Domain.Models;
using TOTVS.WebApi.Controllers;

namespace TOTVSApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {

        public LoginController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(TokenResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401, Type = typeof(ErrorMessage))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _mediator.Send(request);

            if (!response.IsSuccessful)
                return HandleUnsuccessfulResult(response.Errors);

            User user = (User)response.Value;

            var tokenResponse = await _mediator.Send(new BuildTokenRequest() { Subject = user.Email, Audience = "TOTVS.Api" });
            return Ok(tokenResponse);
        }


    }
}
