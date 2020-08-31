using System;
using Application.Activities;
using Application.User;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class BuilderExtensions
    {

        public static IMvcBuilder AddFluentValidationBuilder(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<Create>();
                cfg.RegisterValidatorsFromAssemblyContaining<Login>();
            });

            return builder;
        }
    }
}
