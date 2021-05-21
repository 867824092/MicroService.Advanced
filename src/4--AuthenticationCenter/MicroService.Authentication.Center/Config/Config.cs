namespace MicroService.Authentication.Center {
    using IdentityModel;
    using IdentityServer4.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Config {

        public static List<IdentityServer4.Test.TestUser> TestUsers => new List<IdentityServer4.Test.TestUser> {
            new IdentityServer4.Test.TestUser {
                 Username = "admin",
                  IsActive = true,
                   Password = "1",
                    SubjectId = "1",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Name,"admin"),
                        new Claim(JwtClaimTypes.Email,"123@qq.com")
                    }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResource => new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource> {
            new ApiResource {
                 Name = "Blazor",
                 Scopes  = { "blazor" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope> { 
            new ApiScope {
              DisplayName = "Blazor 客户端",
              Description = "Blazor 客户端链接",
              Name = "blazor"
            }
        };

        public static IEnumerable<Client> Clients =>
           new List<Client>
           {
               new Client
               {
                   ClientId = "blazor",
                   ClientSecrets =
                   {
                       new Secret("secret".Sha256())
                   },
                   AllowedGrantTypes = GrantTypes.Implicit,
                   AllowAccessTokensViaBrowser = true,
                   RequireClientSecret = false,
                   AllowedCorsOrigins =     { "http://localhost:10200" },
                   RedirectUris = {"http://localhost:10200/authentication/login-callback"},
                   PostLogoutRedirectUris = { "http://localhost:10200/authentication/logout-callback" },
                   AllowedScopes = { 
                       "blazor",
                       IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServer4.IdentityServerConstants.StandardScopes.Profile
                   }
               },
           };
    }
}
