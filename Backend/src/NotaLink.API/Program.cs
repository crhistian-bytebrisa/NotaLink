using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using Microsoft.OpenApi.Models;
using NotaLink.Infraestructure.Context;
using NotaLink.Domain.Entities;
using NotaLink.Application.Services;
using NotaLink.Application.Validators;
using System.Text;
using NotaLink.Application.Mapping;
using NotaLink.API.Middlewares;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace NotaLink.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MappingConfig.RegisterMappings();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<NotaLinkContext>(options =>
            {
                options.UseSqlite("Data Source=notalinkdb.db");
            });

            builder.Services.AddControllers();

            builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
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


            // Apartado de autenticación y autorización
            builder.Services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<NotaLinkContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddJwtBearer(opt =>
            {
                opt.MapInboundClaims = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),

                    ClockSkew = TimeSpan.Zero
                };
            });

            MappingConfig.RegisterMappings();

            // Inyección de dependencias para los servicios
            builder.Services.AddScoped<AuthServices>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
