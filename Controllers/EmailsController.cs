using Microsoft.AspNetCore.Mvc;
using AlleyCatBarbers.ViewModels;
using AlleyCatBarbers.Services;
using Microsoft.AspNetCore.Authorization;

namespace AlleyCatBarbers.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    public class EmailsController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailViewModel model)
        {

            if (ModelState.IsValid)
            {
                EmailAttachment emailAttachment = null;
                bool EmailSent = false;
                string Message = null;

                // Send email with attachment if one is included
                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.Attachment.CopyToAsync(ms);
                        emailAttachment = new EmailAttachment
                        {
                            FileName = Path.GetFileName(model.Attachment.FileName),
                            Data = ms.ToArray()
                        };
                    }

                    (EmailSent, Message) = await _emailSender.SendEmailAsync(
                        model.To,
                        model.Subject,
                        model.Message,
                        emailAttachment
                        );
                }
                else // Otherwise send email without attachment
                {
                    (EmailSent, Message) = await _emailSender.SendEmailAsync(
                        model.To,
                        model.Subject,
                        model.Message
                        );
                }

                // Parse email success or failure messages to ViewBag
                if (EmailSent)
                {
                    ViewBag.EmailSent = true;
                    ViewBag.Message = Message;
                }
                else
                {
                    ViewBag.EmailSent = false;
                    ViewBag.Error = Message;
                }
            }
            else
            {
                ViewBag.Error = "Invalid form data";
                ViewBag.EmailSent = false;
            }

            return View(model);
        }
    }
}
