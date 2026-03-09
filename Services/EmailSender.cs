using Azure;
using Azure.Communication.Email;
using EllipticCurve.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlleyCatBarbers.Services;
using System;


namespace AlleyCatBarbers.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailClient _emailClient;
        private readonly string _fromEmail;

        public EmailSender(IConfiguration configuration)
        {
            var connectionString = configuration["AzureCommunicationServices:ConnectionString"];
            _emailClient = new EmailClient(connectionString);
            _fromEmail = configuration["AzureCommunicationServices:FromEmail"];
        }

        public async Task<(bool EmailSent, string Message)> SendEmailAsync(string email, string subject, string htmlMessage, EmailAttachment attachment)
        {
            var emailMessage = new EmailMessage(
                senderAddress: _fromEmail,
                recipientAddress: email,
                content: new EmailContent(subject)
               );

            (emailMessage.Content.PlainText, emailMessage.Content.Html) = (htmlMessage, htmlMessage);

            if (attachment != null)
            {                
                var emailAttachment = new Azure.Communication.Email.EmailAttachment(
                    attachment.FileName,
                    "application/octet-stream",
                    new BinaryData(attachment.Data)
                    );

                emailMessage.Attachments.Add(emailAttachment);
            }

            try
            {
                EmailSendOperation emailSendOperation = await _emailClient.SendAsync(
                WaitUntil.Completed, emailMessage);

                return (true, $"{emailSendOperation.Value.Status}");
            }
            catch (RequestFailedException ex)
            {
                return (false, $"{ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                return (false, $"Email send failed.");
            }
        }

    }
}

