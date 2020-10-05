using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace
{
    [Authorize]
    public class ChatRoomModel : PageModel
    {
        public string UserName { 
            get {
                return User.Claims.Where( c => c.Type == "name").FirstOrDefault()?.Value;
            }
        }
        
        public void OnGet()
        {
            
        }
    }
}
