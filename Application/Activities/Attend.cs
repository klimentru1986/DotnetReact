using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Attend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound,
                        new { Activity = "Activity not found" });

                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _userAccessor.GetCurrentUserName());

                var attendance = await _context.UserActivities.SingleOrDefaultAsync(ua =>
                    ua.ActivityId == activity.Id && ua.AppUserId == user.Id
                );

                if (attendance != null)
                    throw new RestException(HttpStatusCode.BadRequest,
                        new { Activity = "Activity alredy exist" });

                _context.UserActivities.Add(
                    new UserActivity
                    {
                        Activity = activity,
                        AppUser = user,
                        IsHost = false,
                        JoinDate = DateTime.Now
                    }
                );

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return Unit.Value;

                throw new Exception();
            }
        }
    }
}