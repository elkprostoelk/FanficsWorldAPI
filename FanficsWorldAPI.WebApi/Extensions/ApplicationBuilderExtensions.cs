using FanficsWorldAPI.Common.Configurations;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.Core.Services;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Extensions.Hosting;
using System.Text;

namespace FanficsWorldAPI.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddSerilogLogging(this WebApplicationBuilder builder)
        {
            if (Log.Logger is ReloadableLogger)
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .CreateBootstrapLogger();
            }

            builder.Services.AddSerilog(Log.Logger);
        }

        private static void RegisterAuth(this WebApplicationBuilder builder)
        {
            var jwtOptsSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtOptions>(jwtOptsSection);

            var jwtOptions = jwtOptsSection.Get<JwtOptions>();
            ArgumentNullException.ThrowIfNull(jwtOptions);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };
            });

            builder.Services.AddAuthorization();
        }

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<FanficsWorldDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("FanficsWorldDb")));

            builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            builder.RegisterAuth();
            builder.Services.AddCors();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IRepository<User>, Repository<User>>();

            builder.Services.AddScoped<IAuthService, AuthService>();
        }

        public static void RegisterSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FanficsWorldAPI",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT as: **Bearer &lt;token&gt;**"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
