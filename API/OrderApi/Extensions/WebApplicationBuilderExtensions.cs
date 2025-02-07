using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderApi.Models;
using OrderApi.Repositories;
using OrderApi.Repositories.Contracts;
using OrderApi.Services;
using OrderApi.Services.Contracts;
using OrderApi.Settings;

namespace OrderApi.Extensions
{
    internal static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AndResgisterApplication(this WebApplicationBuilder builder)
        {


            builder.Services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("OrderContext"));

#if DEBUG
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
#endif

            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            builder.Services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IOrderService, OrderService>();

            builder.Services.AddOptions<AuthenticationSettings>()
                            .Bind(builder.Configuration.GetSection("AuthenticationSettings"));

            builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value);

            return builder;
        }

        public static WebApplicationBuilder AndAddAuthentication(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["AuthenticationSettings:Issuer"],
                    ValidAudience = builder.Configuration["AuthenticationSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthenticationSettings:SecretKey"]!))
                };
            });            

            return builder;

        }
    }
}