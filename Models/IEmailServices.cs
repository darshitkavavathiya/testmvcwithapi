using System.Threading.Tasks;

namespace testmvc.Models
{
    public interface IEmailServices
    {
        Task SendResetPassword(UserEmailOptions userEmailOptions);
    }
}