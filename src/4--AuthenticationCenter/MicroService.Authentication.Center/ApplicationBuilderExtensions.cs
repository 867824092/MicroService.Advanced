namespace MicroService.Authentication.Center {
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Interfaces;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions {

        public static void UseSeedData(this IApplicationBuilder app) {
            var serviceProvider = app.ApplicationServices;

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope(); scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();
            InitCustomSeedData(context);

            var user = scope.ServiceProvider.GetService<ApplicationDbContext>();
            user.Database.Migrate();
        }

        private static void InitCustomSeedData(ConfigurationDbContext context) {
            if (!context.Clients.Any()) {
                foreach (var client in Config.Clients) {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            if (!context.ApiResources.Any()) {
                foreach (var api in Config.ApiResources)
                    context.ApiResources.Add(api.ToEntity());
                context.SaveChanges();
            }
            if (!context.ApiScopes.Any()) {
                foreach (var api in Config.ApiScopes)
                    context.ApiScopes.Add(api.ToEntity());
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any()) {
                foreach (var api in Config.IdentityResource)
                    context.IdentityResources.Add(api.ToEntity());
                context.SaveChanges();
            }
        }
    }
}
