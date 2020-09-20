using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<PagedDto<ActivityDto>>
        {
            public int? Skip { get; set; }
            public int? Take { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedDto<ActivityDto>>
        {

            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<PagedDto<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var queriable = _context.Activities.AsQueryable();
                var count = queriable.Count();

                var activities = await queriable
                    .Skip(request.Skip ?? 0)
                    .Take(request.Take ?? count)
                    .ToListAsync();

                return new PagedDto<ActivityDto>
                {
                    Data = _mapper.Map<List<Activity>, List<ActivityDto>>(activities),
                    Total = count
                };
            }
        }
    }
}