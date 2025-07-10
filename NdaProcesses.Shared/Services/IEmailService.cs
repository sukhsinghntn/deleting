using DynamicFormsApp.Shared.Models;
using System.Threading.Tasks;

namespace DynamicFormsApp.Shared.Services
{
    public interface IEmailService
    {
        Task SendBugReportEmail(EmailModel email);
        Task SendFormResponseNotification(string toEmail, string formName, int formId);
    }
}
