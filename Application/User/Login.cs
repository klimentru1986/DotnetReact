using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User
{
    public class Login
    {
        public class Query : IRequest<AppUser>
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

        public class Haldler : IRequestHandler<Query, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;

            public Haldler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<AppUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new Exception("Unauthorized");

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if(!result.Succeeded)
                    throw new Exception("Unauthorized");

                return user;
            }
        }
    }
}
