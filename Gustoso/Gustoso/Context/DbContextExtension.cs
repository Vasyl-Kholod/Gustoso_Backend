using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Gustoso.Common.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gustoso.Context
{
    public static class DbContextExtension
    {
        public static async Task EnsureSeeded(this MSContext context, IConfiguration _config, IServiceProvider provider)
        {
            using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!context.Roles.Any())
                {
                    foreach (Roles role in Enum.GetValues(typeof(Roles)))
                    {
                        var identityRole = new IdentityRole(roleName: role.ToString());
                        await roleManager.CreateAsync(identityRole);
                        await roleManager.AddClaimAsync(identityRole, new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString()));
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
