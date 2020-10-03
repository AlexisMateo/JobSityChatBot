using Microsoft.EntityFrameworkCore;

namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
    }
}