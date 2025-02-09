using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var userName = Context.User?.Claims?.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier
            )?.Value;

            command.UserName = userName;

            var comment = _mediator.Send(command);
        
            await Clients.All.SendAsync("Comment", comment);
        }
    }
}