using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JobSity.ChatApp.IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if(result.Succeeded)
                {
                    return Redirect("Index");
                }
            }

            return View();
        }

        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = string.Empty
            };

            var result = await _userManager.CreateAsync(user, password);

            if(result.Succeeded)
            {

            }

            return View();
        }

    }
}