using System;
using System.Threading.Tasks;
using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> register(Register.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
