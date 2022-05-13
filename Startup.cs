using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Text;
using testmvc.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace testmvc
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
            services.AddControllersWithViews();
            services.AddMvc();


            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]);

                        services.AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                         })
                           .AddJwtBearer(x =>
                           {
                               x.RequireHttpsMetadata = false;
                               x.SaveToken = true;
                               x.TokenValidationParameters = new TokenValidationParameters {
                                   ValidateIssuerSigningKey = true,
                                   IssuerSigningKey = new SymmetricSecurityKey(key),
                                   ValidIssuers = new string[] { Configuration["Jwt:Issuer"] },
                                   ValidAudiences = new string[] { Configuration["Jwt:Issuer"] },
                                   ValidateIssuer = true,
                                   ValidateAudience = true,
                                   ValidateLifetime = true
                               };
                           });









            services.AddTransient<IUser, UserServices>();
            services.AddTransient<ICustomer, CustomerServices>();
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddSession();
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<SMTPconfig>(Configuration.GetSection("SMTPconfig"));
            services.Configure<Jwt>(Configuration.GetSection("Jwt"));
           




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
           

            app.UseSession();
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authentication}/{action=Login}/{id?}");
            });
        }
    }
}
