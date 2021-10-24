using Azure.Storage.Blobs;
using MapBanana.Api.Configuration;
using MapBanana.API.AuthorizationRequirements;
using MapBanana.API.ICampaignDatabase;
using MapBanana.API.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1#globally-require-all-users-to-be-authenticated-1

namespace MapBanana.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ApiConfiguration ApiConfiguration { get; } = new ApiConfiguration();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.GetSection(nameof(ApiConfiguration)).Bind(ApiConfiguration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration.
            services.AddOptions<ApiConfiguration>()
                .Bind(Configuration.GetSection(nameof(ApiConfiguration)));

            services.AddSingleton(ApiConfiguration);

            // Authorization.
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new AllowConfigurationRequirement(ApiConfiguration))
                .Build();
            });
            services.AddSingleton<IAuthorizationHandler, AllowConfigurationHandler>();

            // Controllers.
            services.AddControllers();

            // CORS.
            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(new string[]
                    {
                        "http://localhost:4200",
                        "https://localhost:4200"
                    });
                });
            });

            // Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapBanana", Version = "v1" });

                // Auth0 authorization.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Open Id" }
                            },
                            AuthorizationUrl = new Uri($"{ApiConfiguration.AuthenticationDomain}authorize?audience={ApiConfiguration.AuthenticationAudience}")
                        }
                    }
                });
                c.OperationFilter<SwaggerAuthorizeOperationFilter>();
            });

            // Auth0 authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = ApiConfiguration.AuthenticationDomain;
                options.Audience = ApiConfiguration.AuthenticationAudience;
            });

            // Hub connection.
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl(ApiConfiguration.EventHubCampaignUrl)
                .WithAutomaticReconnect(ApiConfiguration.EventHubReconnectDelays)
                .Build();

            hubConnection.StartAsync().Wait();
            services.AddSingleton(hubConnection);

            // Storage account.
            BlobContainerClient blobContainerClient = new BlobContainerClient(ApiConfiguration.StorageConnectionString, ApiConfiguration.StorageCampaignRootName);
            services.AddSingleton(blobContainerClient);
            services.AddSingleton<ICampaignStorage, AzureBlobCampaignStorage>();

            // Database.
            services.AddSingleton<ICampaignDatabase, AzureSqlCampaignDatabase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapBanana v1");
                    c.OAuthClientId(ApiConfiguration.AuthenticationClientId);
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
