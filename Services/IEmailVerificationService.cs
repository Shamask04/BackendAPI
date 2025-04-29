using System.Threading.Tasks;

namespace PortfolioTrackerApi.Services
{
    public interface IEmailVerificationService
    {
        Task SendVerificationCode(string email); // Now async
        bool VerifyCode(string email, string code);
    }
}
