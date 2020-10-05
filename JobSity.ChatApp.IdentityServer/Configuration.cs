using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace JobSity.ChatApp.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>{
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        public static IEnumerable<ApiResource> GetApisResources()
        {
            return new List<ApiResource> { 
                new ApiResource("ChatApi", "Chat Api"){
                    Scopes = new List<string>(){
                        "chatapi.access"
                    }
                } 
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>{
                new Client{
                    ClientId = "chatClient",
                    ClientSecrets = { new Secret("chatClientSecret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ChatApi", "chatapi.access" }
                },
                new Client{
                    ClientId = "chatWebClient",
                    ClientSecrets = { new Secret("chatWebClientSecret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5003/signin-oidc" },
                    AllowedScopes = { "ChatApi", "chatwebapi.access"
                    , IdentityServerConstants.StandardScopes.OpenId
                    , IdentityServerConstants.StandardScopes.Profile                    
                     },
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>{
                new ApiScope(name: "chatapi.access",   displayName: "Access Chat API")
            };
        }
    }
}