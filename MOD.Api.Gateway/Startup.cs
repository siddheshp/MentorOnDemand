using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace MOD.Api.Gateway
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
            services.AddCors();
            services.AddControllers();
            services.AddOcelot(Configuration);

            ////JWT
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme =
            //        JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme =
            //        JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme =
            //        JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(cfg =>
            //    {
            //        cfg.Authority = "https://localhost:44336"; //identity url
            //        cfg.RequireHttpsMetadata = false;
            //        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //        {
            //            ValidAudiences = new[] { "admin", "mentor", "student" }
            //        };

            //        //cfg.SaveToken = true;
            //        //cfg.TokenValidationParameters = new TokenValidationParameters
            //        //{
            //        //    ValidIssuer = Configuration["JwtIssuer"],
            //        //    ValidAudience = Configuration["JwtIssuer"],
            //        //    IssuerSigningKey = new SymmetricSecurityKey(
            //        //        Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
            //        //    ClockSkew = TimeSpan.Zero // remove delay of token when expire
            //        //};
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(policy => policy.AllowAnyOrigin()
                                .AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await app.UseOcelot();
        }
    }
}
