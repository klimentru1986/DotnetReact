using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class LoginValidator : AbstractValidator<Query>
        {
            public LoginValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("email обязателен для заполненя");
                RuleFor(x => x.Password).NotEmpty().WithMessage("пароль обязателен для заполненя");
            }
        }

        public class Haldler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Haldler(
                UserManager<AppUser> userManager, 
                SignInManager<AppUser> signInManager,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.Unauthorized);

                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    UserName = user.UserName,
                    Image = null
                };
            }
        }
    }
}
