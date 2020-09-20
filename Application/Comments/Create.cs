using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string Data { get; set; }
            public Guid ActivityId { get; set; }

            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {

                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.UserName);
                var activity = await _context.Activities.FindAsync(request.ActivityId);

                var comment = new Comment
                {
                    Data = request.Data,
                    CreateDate = DateTime.Now,
                    Author = user,
                    Activity = activity
                };

                _context.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return _mapper.Map<Comment, CommentDto>(comment);

                throw new System.Exception("Save cahnges problem");
            }
        }
    }
}