namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class FinancialChatBaseRepository<T> : BaseRepository<T, FinancialChatDbContext> where T : class
    {
        public FinancialChatBaseRepository(FinancialChatDbContext dbContext):base(dbContext)
        { 
        }
    }
}