using JobSity.ChatApp.Core.Entities.Chat;
using Microsoft.EntityFrameworkCore;

namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class FinancialChatDbContext : DbContext
    {
        DbSet<Message> Messages { get; set; }
        protected FinancialChatDbContext(DbContextOptions<FinancialChatDbContext> options) : base(options)
        {
            
        }
    }
}