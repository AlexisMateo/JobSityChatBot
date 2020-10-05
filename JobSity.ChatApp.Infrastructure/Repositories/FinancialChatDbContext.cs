using JobSity.ChatApp.Core.Entities.Chat;
using Microsoft.EntityFrameworkCore;

namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class FinancialChatDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public FinancialChatDbContext(DbContextOptions<FinancialChatDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=localhost;Database=FinancialChatDb;User Id=sa;Password=dev123*1;");
        }
    }
}