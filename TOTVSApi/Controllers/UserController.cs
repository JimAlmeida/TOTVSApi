using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTVS.Domain;
using TOTVS.Domain.Entities;
using TOTVS.Domain.Models;
using TOTVS.Domain.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TOTVS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator) { }

        // GET: api/<UserController>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> GetUsersAsync()
        {
            var response = await _mediator.Send(new GetUsersQuery());

            return (response.IsSuccessful) ? Ok(response.Value) : HandleUnsuccessfulResult(response.Errors);
        }

        // GET api/<UserController>/5
        [Authorize]
        [HttpGet("{identifier}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(ErrorMessage))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> GetUserAsync([FromRoute] string identifier)
        {
            var response = await _mediator.Send(new GetUserByIdQuery() { UserIdentifier = identifier });

            return (response.IsSuccessful) ? Ok(response.Value) : HandleUnsuccessfulResult(response.Errors);
        }

        [Authorize]
        [HttpPut("{identifier}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorMessage))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] string identifier, [FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _mediator.Send(new UpdateUserRequest() { User = user });

            return (response.IsSuccessful) ? NoContent() : HandleUnsuccessfulResult(response.Errors);
        }

        [Authorize]
        [HttpDelete("{identifier}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorMessage))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string identifier)
        {
            var response = await _mediator.Send(new DeleteUserRequest() { Identifier = identifier });

            return (response.IsSuccessful) ? NoContent() : HandleUnsuccessfulResult(response.Errors); ;
        }

        // POST api/<UserController>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(422, Type = typeof(ErrorMessage))]
        [ProducesResponseType(500, Type = typeof(ErrorMessage))]
        public async Task<IActionResult> RegisterNewUserAsync([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var response = await _mediator.Send(request);

            if (!response.IsSuccessful)
                return HandleUnsuccessfulResult(response.Errors);

            User user = (User)response.Value;
            return CreatedAtAction("RegisterNewUser", new { identifier = user.Identifier }, user);
        }

        

    }
}
