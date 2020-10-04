using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace JobSity.ChatApp.IdentityServer
{
    public static class Configuration
    {
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