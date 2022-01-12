using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TOTVS.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MediatR;
using TOTVS.Application.Command;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TOTVS.Domain.Entities;
using TOTVS.WebApi.Middleware;

namespace TOTVSApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddScoped<GlobalExceptionMiddleware>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TOTVSApi", Version = "v1" });
            });

            var connectionString = Configuration.GetConnectionString("PostgreSqlDevelopmentDatabase");
            services.AddDbContext<TOTVSDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                }
            );

            services.AddTransient<IDbConnection>(_ => { return new NpgsqlConnection(connectionString); });
            services.AddOptions<PasswordHasherOptions>().Configure(cfg => cfg.IterationCount = 300000);
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>(provider => { return new PasswordHasher<User>(provider.GetRequiredService<IOptions<PasswordHasherOptions>>()); });

            services.AddMediatR(typeof(LoginCommandHandler));

            var jwtKey = Encoding.UTF8.GetBytes(Configuration.GetSection("SecurityKeys")["JwtSymmetricalKey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                    ValidIssuer = "AlmeidaDev",
                    ValidAudience = "TOTVS.Api"
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
     
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TOTVSApi v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
