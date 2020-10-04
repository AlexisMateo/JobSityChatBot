using System.Threading.Tasks;
using JobSity.ChatApp.Core.Entities.Identity;

namespace JobSity.ChatApp.Core.Interfaces.Identity
{
    public interface IIdentityManagerService
    {
        Task<string> GetAccessToken(BasicTokenRequest tokenRequest);
    }
}