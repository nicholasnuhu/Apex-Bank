using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Payment.Core.Interfaces;
using Payment.Core.Services;
using Payment.Core.Utilities.PaymentGatewaySettings;
using Payment.Core.Utilities.Profiles;
using Payment.Core.Utilities.Settings;
using Payment.Domain.Models;
using Payment.Infrastructure;
using Serilog;
using System.Reflection;
using System.Text;

namespace Payment.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            // This method gets called by the runtime from the startup "ConfigureServices()" to add swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Payment - WebApi",
                    Description = "This Api will be responsible for handling payments",
                    Contact = new OpenApiContact
                    {
                        Name = "Fintech Web-API",
                        Email = "",
                    }
                });
                // To Enable authorization using Swagger (JWT) 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        public static void AddSeriLogExtension(this IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);
        }

        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));

            // Register dependencies here
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<ApplicationSettings>>().Value);
            services.AddScoped<IPayStackPaymentHandler, PayStackPaymentHandler>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IVirtualAccountService, VirtualAccountService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<PaymentDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSettings");
            var secretKey = jwtSetting["Secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSetting["ValidIssuer"],
                        ValidAudience = jwtSetting["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }
    }
}