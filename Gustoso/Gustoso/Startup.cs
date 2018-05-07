using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Gustoso.Context;
using Gustoso.Common.IServices;
using Gustoso.Common.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Gustoso.Services;
using System.Security.Claims;
using Gustoso.Common.Enums;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using Gustoso.Socket;
using Microsoft.AspNetCore.Authorization;

namespace Gustoso
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddAuthentication(p => p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                }); ;

            services.AddDbContext<MSContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Gustoso"));
            }, ServiceLifetime.Transient);

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MSContext>();

            services
                .AddMvc()
                .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
                options.SerializerSettings.FloatParseHandling = FloatParseHandling.Decimal;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSingleton(Configuration);

            services.AddSingleton<IGustosoSocketServer, GustosoSocketServer>();

            services.AddTransient<IPriceListService, PriceListService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IContactUsService, ContactUsService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IReservationService, ReservationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            await EnsureDataBaseReady(provider);
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            provider.CreateScope().ServiceProvider.GetRequiredService<IGustosoSocketServer>();
        }

        private async Task EnsureDataBaseReady(IServiceProvider provider)
        {
            using (var context = provider.CreateScope().ServiceProvider.GetRequiredService<MSContext>())
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception)
                {
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                }

                if (!context.MenuItems.Any())
                {
                    var priceList = new List<PriceItem>();
                    using (StreamReader sr = new StreamReader("PriceList.json"))
                    {
                        var json = await sr.ReadToEndAsync();
                        priceList = JsonConvert.DeserializeObject<List<PriceItem>>(json);
                    }

                    foreach (var priceItem in priceList)
                    {
                        var menuItem = new MenuItem
                        {
                            dish = priceItem.dish,
                            ingredients = priceItem.ingredients,
                            price = priceItem.price,
                            weight = priceItem.weight
                        };
                        context.MenuItems.Add(menuItem);
                    }
                    await context.SaveChangesAsync();

                }

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

                if (!context.Users.Any())
                {
                    using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                        DateTime dateNow = DateTime.UtcNow;

                        var user = new User
                        {
                            Email = "gustosobackery@gmail.com",
                            UserName = "gustosobackery@gmail.com",
                            FirstName = "Gustoso",
                            LastName = "Backery",
                            Phone = "380000000000",
                            DateOfRegistration = dateNow.ToString()
                        };

                        // добавляем пользователя
                        var resultCreatedUser = await _userManager.CreateAsync(user, "Gustoso_99");
                        var resultCreatedRole = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
