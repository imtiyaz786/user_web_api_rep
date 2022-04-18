using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USERWebApi;
using USERWebApi.Models;
using USERWebApi.Repository;
namespace USERWebApi
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:3000", "http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });

            services.AddScoped<IRepository<Doctor>, DoctorRepository<Doctor>>();

            services.AddScoped<IRepository<Patient>, PatientRepository<Patient>>();

            services.AddScoped(typeof(AppSeedAdmin));

            services.AddControllers();

            services.AddDbContext<UserDbContext>(setup => setup.UseSqlServer(Configuration.GetConnectionString("DbConnect")));
            
            services.AddIdentity<RegistrationModel, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("UserWebApi",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "User Web Api",
                        Version = "1",
                        Description = "User Authentication"
                    });
            });
            string key = Configuration["JWT:Key"];
            string issuer = Configuration["JWT:Issuer"];
            string audience = Configuration["JWT:Audience"];

            byte[] KeyBytes = System.Text.Encoding.ASCII.GetBytes(key);
            SecurityKey securityKey = new SymmetricSecurityKey(KeyBytes);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            services.AddAuthentication(setup=>
            {
                setup.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
                setup.DefaultForbidScheme=JwtBearerDefaults.AuthenticationScheme;
                setup.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                setup.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                setup.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(setup=>setup.TokenValidationParameters=new TokenValidationParameters
            {
                ValidateIssuer = true,  
                ValidateAudience = true,    
                ValidateIssuerSigningKey = true,    
                ValidAudience = audience,
                ValidIssuer=issuer, 
                IssuerSigningKey=securityKey
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseSwagger();
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/UserWebApi/swagger.json", "USERWebApi");
                options.RoutePrefix = "";
            }); 

            app.UseAuthentication();    

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
