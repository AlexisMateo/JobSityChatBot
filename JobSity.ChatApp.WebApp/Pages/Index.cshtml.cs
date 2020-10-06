using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using JobSity.ChatApp.Core.Interfaces.Identity;
using JobSity.ChatApp.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;

namespace JobSity.ChatApp.WebApp.Pages
{
     [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IIdentityManagerService _identityManagerService;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger
            , IIdentityManagerService identityManagerService
            , IConfiguration configuration
            , IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _identityManagerService = identityManagerService;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
            throw new Exception("webapp test exception");
        }

        public async Task<IActionResult> OnGetRetrieveChatApiUrlAsync()
        {
            var identityInfo = _configuration.GetSection("IdentityInfo");
            var chatApiUrl = _configuration.GetSection("ChatpApiUrl").Value;
            
            var basicTokenRequest = new BasicTokenRequest
            {
                ClientId = identityInfo.GetSection("ClientId").Value,
                ClientSecret = identityInfo.GetSection("ClientSecret").Value,
                Address = identityInfo.GetSection("Address").Value,
                Scope = identityInfo.GetSection("Scope").Value
            };

            var accessToken = await _identityManagerService.GetAccessToken(basicTokenRequest);
            
            return new JsonResult($"{chatApiUrl}?token={accessToken}");
        }
    }
}
