using System.Text;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shinsekai_API.Config;
using Shinsekai_API.Models;
using Shinsekai_API.Services;

namespace Shinsekai_API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ApiConfiguration();
            
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                c.AddPolicy(name: "shinsekai", policy =>
                {
                    policy.WithOrigins("https://sinsekai.mx");
                });
            });

            services.AddAuthentication(r =>
            {
                r.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                r.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001"
                };
            });
            services.AddDbContext<ShinsekaiApiContext>(options => options.UseSqlServer(configuration.ConnectionString));
            services.AddControllers();
            services.AddMvc()
                .AddNewtonsoftJson(
                    options => {
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
                        });
            services.AddSingleton(r => new BlobServiceClient(configuration.BlobStorageConnectionString));
            services.AddSingleton<IBlobService, BlobStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseCors("shinsekai");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

}