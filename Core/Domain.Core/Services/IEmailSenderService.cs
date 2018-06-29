using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Domain.Core.Services {
    public interface IEmailSenderService {
        void SendEmail(string subject, string body, MailAddress from, IEnumerable<MailAddress> to, IEnumerable<AlternateView> embeddedResources, IEnumerable<string> bcc, IEnumerable<string> cc, IEnumerable<Attachment> attachments);
    }
}
