using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YouTubeFullApplication.BusinessLayer;
using YouTubeFullApplication.Shared;
using YouTubeFullApplication.Validation;
using YouTubeFullApplication.Json;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

namespace YouTubeFullApplication.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var jsonOptions = builder.Services.AddJsonOptions();
            builder.Services.AddControllers().AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
                config.JsonSerializerOptions.DefaultIgnoreCondition = jsonOptions.DefaultIgnoreCondition;
                config.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
                config.JsonSerializerOptions.Converters.Clear();
                foreach (var converter in jsonOptions.Converters)
                {
                    config.JsonSerializerOptions.Converters.Add(converter);
                }
            });
            builder.Services.AddProblemDetails();

            // Sistema per la generazione documentazione JSON di Microsoft
            // builder.Services.AddOpenApi();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // Sistema per la generazione documentazione JSON di Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert the Bearer Token",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey
                });
                var openApiSecurityRequirement = new OpenApiSecurityRequirement { {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme }},
                    Array.Empty<string>() } };
                options.AddSecurityRequirement(openApiSecurityRequirement);
            });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidation();
            ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) => member?.Name.FirstLower();


            JwtSettings jwtSettings = builder.Services.AddBusinessLayer(builder.Configuration);

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.IncludeErrorDetails = true;
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Usiamo documentazione Microsoft
                // app.MapOpenApi();
                //app.UseSwaggerUI(options => {
                //    options.SwaggerEndpoint("/openapi/v1.json", app.Environment.ApplicationName);
                //});

                // Usiamo documentazione Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseWebAssemblyDebugging(); // Per blazor
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles(); // per blazor
            app.UseStaticFiles(); // per blazor e anche altro

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
