

namespace AlleyCatBarbers.Services
{
    public interface IEmailSender
    {
        //Task<(bool EmailSent, string Message)> SendEmailAsync(string email, string subject, string message);
        Task<(bool EmailSent, string Message)> SendEmailAsync(string email, string subject, string message, 
            EmailAttachment attachment = null);
    }
}
