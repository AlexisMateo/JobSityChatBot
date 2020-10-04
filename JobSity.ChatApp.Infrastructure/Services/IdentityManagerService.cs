using System.Net.Http;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Interfaces.Identity;
using IdentityModel.Client;
using JobSity.ChatApp.Core.Entities.Identity;

namespace JobSity.ChatApp.Infrastructure.Services
{
    public class IdentityManagerService : IIdentityManagerService
    {
        private readonly HttpClient _httpClient;

        public IdentityManagerService(HttpClient httpClient)
        {
            _httpClient = httpClient;    
        }

        public async Task<string> GetAccessToken(BasicTokenRequest tokenRequest)
        {
            var discoveredDocument = await _httpClient.GetDiscoveryDocumentAsync(tokenRequest.Address);
            
            var tokeRequest = new TokenRequest();
            
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
             
                new ClientCredentialsTokenRequest{

                    Address = discoveredDocument.TokenEndpoint,
                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.ClientSecret,
                    Scope = tokenRequest.Scope

                }
            );
            
            return tokenResponse.AccessToken;

        }
    }
}