using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Voting.Api.Mapping;
using Voting.Core.Configuration;
using Voting.Core.Entities;
using Voting.Core.Hubs;
using Voting.Core.Interfaces;
using Voting.Core.Interfaces.Services;
using Voting.Core.Services;
using Voting.Data.EF;
using Voting.Data.Repositories;

namespace Voting.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging();
            });
            services.AddIdentity<User, IdentityRole>(config => 
            {
                config.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(configure =>
                {
                    configure.RequireHttpsMetadata = true;
                    configure.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JwtIssuerOptions:Issuer"],
                        ValidAudience = Configuration["JwtIssuerOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtIssuerOptions:Key"]))
                    };
                    configure.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/comment-hub")))
                                context.Token = accessToken;
                            
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();

            services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);
            services.AddHttpContextAccessor();

            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(options => {
                    options.SuppressModelStateInvalidFilter = true;
                });
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QuestionHub>("/question-hub");
                endpoints.MapHub<CommentHub>("/comment-hub");
            });
        }
    }
}