using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NeHauar.Contexts;
using NeHauar.Helpers;
using NeHauar.Hubs;
using NeHauar.Models;
using NeHauar.Services;

namespace NeHauar
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        public readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
                services.AddDbContext<DataContext>();
            else
                services.AddDbContext<DataContext, SqliteDataContext>();
            
            services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("NeHauar"));
            services.AddSignalR();
            services.AddControllers();
            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpContextAccessor();

            // configure strongly typed settings object
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = int.Parse(context.Principal.Identity.Name);
                            var user = userService.GetById(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatService, ChatService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            dataContext.Database.Migrate();

            // app.UseHttpsRedirection();

            app.UseCors(x => x.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });
        }
    }
}
