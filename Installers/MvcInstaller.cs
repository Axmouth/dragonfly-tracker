﻿using DragonflyTracker.Authorization;
using DragonflyTracker.Filters;
using DragonflyTracker.Options;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using DragonflyTracker.Helpers;

namespace DragonflyTracker.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddScoped<IIdentityService, IdentityService>();
            // services.AddDbContextPool<>

            services.AddAntiforgery(options =>
            {
                var cookieBuilder = new CookieBuilder()
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.None,
                    Name = "X-XSRF-TOKEN"
                    
                };
                // options.Cookie = cookieBuilder;
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddControllersWithViews(options =>
                {
                    options.Filters.Add(new ValidationFilter());
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.IgnoreNullValues = true;
                 })
                .AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddRouting(options =>
            {
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // ...
                options.SuppressModelStateInvalidFilter = true;
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("google", policy =>
                    {
                        policy.AddRequirements(new WorksForCompanyRequirement("google.com"));
                    });
            });

            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new RestfulUriService(absoluteUri, accessor);
            });

            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            services.AddScoped<IMailService>(provider =>
            {
                var renderer = provider.GetRequiredService<IRazorViewToStringRenderer>();
                EmailSettings emailSettings = new EmailSettings();
                configuration.GetSection(nameof(EmailSettings)).Bind(emailSettings);
                return new MailService(emailSettings, renderer);
            });
        }
    }
}
