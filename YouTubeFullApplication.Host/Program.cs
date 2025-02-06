using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using YouTubeFullApplication.BusinessLayer;
using YouTubeFullApplication.Json;
using YouTubeFullApplication.Shared;
using YouTubeFullApplication.Validation;

namespace YouTubeFullApplication.Host
{
    public class JwtOpenApiDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            // Aggiunge lo schema di autenticazione JWT
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Inserisci il token JWT nel formato: Bearer {token}"
            };

            // Applica il requisito di sicurezza globale agli endpoint protetti
            document.SecurityRequirements.Add(new OpenApiSecurityRequirement
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
                new List<string>()
            }
        });
            return Task.CompletedTask;
        }
    }

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
            // aggiungendo la parte per l'autenticazione
            builder.Services.AddOpenApi(options => {
                options.AddDocumentTransformer<JwtOpenApiDocumentTransformer>();
            });
            

            // Sistema per la generazione documentazione JSON di Swagger con autenticazione
            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Insert the Bearer Token",
            //        Name = HeaderNames.Authorization,
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    var openApiSecurityRequirement = new OpenApiSecurityRequirement { {
            //        new OpenApiSecurityScheme {
            //            Reference = new OpenApiReference {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = JwtBearerDefaults.AuthenticationScheme }},
            //        Array.Empty<string>() } };
            //    options.AddSecurityRequirement(openApiSecurityRequirement);
            //});

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidation();
            ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) => member?.Name.FirstLower();


            JwtSettings jwtSettings = builder.Services.AddBusinessLayer(builder.Configuration);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
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
                // Usiamo documentazione Microsoft con UI Swagger
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", app.Environment.ApplicationName);
                });

                // oppure

                // Usiamo documentazione e UI Swagger
                //app.UseSwagger();
                //app.UseSwaggerUI();

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
