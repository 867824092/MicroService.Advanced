namespace MicroService.Authentication.Center {
    using IdentityServer4.Configuration;
    using IdentityServer4.EntityFramework.DbContexts;
    using MicroService.Authentication.Center.Domain;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class Startup {
        IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            //services.AddCookiePolicy(options => {

            //    });
            var identityServerBuilder =   services.AddIdentityServer();
            if (Environment.IsDevelopment()) {
                identityServerBuilder.AddDeveloperSigningCredential(true);
            } else {
                identityServerBuilder.AddSigningCredential(new X509Certificate2(Path.Combine(Environment.ContentRootPath,
                  Configuration["Certificates:CertPath"]),
                  Configuration["Certificates:Password"]));
            }
            identityServerBuilder.AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryIdentityResources(Config.IdentityResource)
            .AddTestUsers(Config.TestUsers);

            #region 集成 efcore aspnetIdentity身份验证
        //services.AddDbContext<ApplicationDbContext>(options =>
        // options.UseSqlite("Data Source=IdentityServer.db"));

        //services.AddIdentity<ApplicationUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();

        //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        //services
        //    .AddIdentityServer(options => {
        //        options.Events.RaiseErrorEvents = true;
        //        options.Events.RaiseInformationEvents = true;
        //        options.Events.RaiseFailureEvents = true;
        //        options.Events.RaiseSuccessEvents = true;

        //        options.UserInteraction = new UserInteractionOptions {
        //            LogoutUrl = "/Account/Logout",
        //            LoginUrl = "/Account/Login",
        //            LoginReturnUrlParameter = "returnUrl"
        //        };
        //    })
        //    .AddDeveloperSigningCredential(true)
        //    .AddConfigurationStore(options =>
        //     {
        //         options.ConfigureDbContext = db =>
        //             db.UseSqlite("Data Source=IdentityServer.db",
        //                 sql => sql.MigrationsAssembly(migrationsAssembly));
        //     })
        //    .AddOperationalStore(options => {
        //        options.ConfigureDbContext = db =>
        //            db.UseSqlite("Data Source=IdentityServer.db",
        //                sql => sql.MigrationsAssembly(migrationsAssembly));
        //        options.EnableTokenCleanup = true;
        //        options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
        //    })
        //    .AddAspNetIdentity<ApplicationUser>();//集成AspNetIdentity身份验证
        #endregion

    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseSerilogRequestLogging(); // <-- Add this line
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseSeedData();
        }
    }
}
