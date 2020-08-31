using System;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using FluentValidation.AspNetCore;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager<SignInManager<AppUser>>();

            return services;
        }

    }
}
