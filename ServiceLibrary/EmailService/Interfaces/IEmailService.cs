using ServiceLibrary.EmailService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.EmailService.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
        Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct);
        string GetEmailTemplate<T>(T emailTemplateModel, string path);
    }
}
