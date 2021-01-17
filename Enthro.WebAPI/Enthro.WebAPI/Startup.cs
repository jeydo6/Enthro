using Enthro.Application.Configs;
using Enthro.Domain.Entities;
using Enthro.Domain.Factories;
using Enthro.Domain.Repositories;
using Enthro.Persistence.DbContexts;
using Enthro.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enthro.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            AuthOptions = Configuration
                .GetSection("EndpointConfig")
                .Get<EndpointConfig>();
        }

        public IConfiguration Configuration { get; }

        private EndpointConfig AuthOptions { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EnthroDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("EnthroConnection")
                )
            );

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services
                .AddOptions();

            services
                .Configure<EndpointConfig>(
                    Configuration.GetSection("EndpointConfig")
                );

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.SignIn = new SignInOptions
                    {
                        RequireConfirmedEmail = false
                    };

                    options.Password = new PasswordOptions
                    {
                        RequiredLength = 6,
                        RequireDigit = false,
                        RequireNonAlphanumeric = false,
                        RequireUppercase = false,
                        RequireLowercase = false
                    };

                    options.User = new UserOptions
                    {
                        RequireUniqueEmail = true
                    };

                    options.Lockout = new LockoutOptions
                    {
                        DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5)
                    };
                })
                .AddEntityFrameworkStores<EnthroDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudiences = AuthOptions.Audiences,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.Secret))
                    };
                });

            services
                .AddScoped<IIndicatorsRepository, IndicatorsRepository>();

            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("main", new OpenApiInfo
                    {
                        Version = System.Reflection.Assembly
                            .GetEntryAssembly()
                            .GetName()
                            .Version
                            .ToString(),
                        Title = "EnthroAPI",
                        Description = "Enthro WebAPI"
                    });

                    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                Scheme = "oauth2",
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            new List<String>()
                        }
                    });
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "";
                options.SwaggerEndpoint("/swagger/main/swagger.json", "EnthroAPI");
            });
        }
    }
}
