using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class IdentityChatDbContext : IdentityDbContext
    {
        public IdentityChatDbContext(DbContextOptions<IdentityChatDbContext> options) : base(options)
        {

        }
    }
}