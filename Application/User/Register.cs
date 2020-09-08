using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.User
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {

                if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "пользователь с такой почтой уже существует" });

                if (await _context.Users.AnyAsync(x => x.UserName == request.UserName))
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = "пользователь с таким именем уже существует" });

                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    UserName = request.UserName,
                    Email = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);


                if (result.Succeeded)
                {
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        UserName = user.UserName,
                        Token = _jwtGenerator.CreateToken(user)
                    };
                }

                throw new System.Exception("Save cahnges problem");
            }
        }
    }
}